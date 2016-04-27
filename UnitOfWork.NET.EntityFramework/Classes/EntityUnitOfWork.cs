﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnitOfWork.NET.EntityFramework.Interfaces;

namespace UnitOfWork.NET.EntityFramework.Classes
{
    public class EntityUnitOfWork : NET.Classes.UnitOfWork, IEntityUnitOfWork
    {
        private readonly bool _autoContext;
        private readonly DbContext _dbContext;

        public EntityUnitOfWork(DbContext context) : this(context, false)
        {
        }

        internal EntityUnitOfWork(DbContext context, bool managedContext)
        {
            _dbContext = context;
            _autoContext = managedContext;
        }

        public override void Dispose()
        {
            if (_autoContext)
                _dbContext.Dispose();

            base.Dispose();
        }

        public override IEnumerable<T> Data<T>() => Set<T>().AsEnumerable();

        public virtual void BeforeSaveChanges(DbContext context)
        {
        }

        public int SaveChanges()
        {
            _dbContext.ChangeTracker.DetectChanges();

            var entries = _dbContext.ChangeTracker.Entries();
            var entriesGroup = entries.Where(t => t.State != EntityState.Unchanged && t.State != EntityState.Detached).ToList().GroupBy(t => ObjectContext.GetObjectType(t.Entity.GetType())).ToList().Select(t => new { t.Key, EntriesByState = t.GroupBy(g => g.State, g => g.Entity).ToList() }).ToList();

            BeforeSaveChanges(_dbContext);

            var res = _dbContext.SaveChanges();

            foreach (var item in entriesGroup)
            {
                var entityType = item.Key;
                var entitiesByState = item.EntriesByState.ToDictionary(t => t.Key, t => t.AsEnumerable());
                var mHelper = GetType().GetMethod("CallOnSaveChanges", BindingFlags.NonPublic | BindingFlags.Instance);
                mHelper.MakeGenericMethod(entityType).Invoke(this, new object[] { entitiesByState });
            }

            AfterSaveChanges(_dbContext);

            return res;
        }

        public virtual void AfterSaveChanges(DbContext context)
        {
        }

        public void Transaction(Action<IEntityUnitOfWork> body)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    body.Invoke(this);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public bool TransactionSaveChanges(Action<IEntityUnitOfWork> body)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    body.Invoke(this);
                    var res = SaveChanges() != 0;
                    transaction.Commit();
                    return res;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class => _dbContext.Entry(entity);

        [Obsolete("Use Set instead")]
        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class => _dbContext.Set<TEntity>();

        public DbSet<TEntity> Set<TEntity>() where TEntity : class => _dbContext.Set<TEntity>();

        public IEntityRepository<TEntity> EntityRepository<TEntity>() where TEntity : class => Repository<TEntity>() as IEntityRepository<TEntity>;

        public IEntityRepository<TEntity, TDTO> EntityRepository<TEntity, TDTO>() where TEntity : class where TDTO : class => Repository<TEntity, TDTO>() as IEntityRepository<TEntity, TDTO>;

        private void CallOnSaveChanges<TEntity>(Dictionary<EntityState, IEnumerable<object>> entitiesObj) where TEntity : class
        {
            var entities = entitiesObj.ToDictionary(t => t.Key, t => t.Value.Cast<TEntity>());

            EntityRepository<TEntity>().OnSaveChanges(entities);
        }

        public async Task<int> SaveChangesAsync() => await new TaskFactory().StartNew(SaveChanges);
    }

    public class EntityUnitOfWork<TContext> : EntityUnitOfWork where TContext : DbContext, new()
    {
        public EntityUnitOfWork() : base(new TContext(), true)
        {
        }

        public virtual void BeforeSaveChanges(TContext context)
        {
            BeforeSaveChanges((DbContext)context);
        }

        public virtual void AfterSaveChanges(TContext context)
        {
            AfterSaveChanges((DbContext)context);
        }
    }
}