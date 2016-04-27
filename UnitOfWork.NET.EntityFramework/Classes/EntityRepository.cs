﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Caelan.DynamicLinq.Classes;
using Caelan.DynamicLinq.Extensions;
using ClassBuilder.Classes;
using ClassBuilder.Interfaces;
using UnitOfWork.NET.Classes;
using UnitOfWork.NET.EntityFramework.Interfaces;
using UnitOfWork.NET.Interfaces;

namespace UnitOfWork.NET.EntityFramework.Classes
{
    public class EntityRepository<TEntity> : Repository<TEntity>, IEntityRepository<TEntity> where TEntity : class
    {
        public EntityRepository(IUnitOfWork manager) : base(manager)
        {
        }

        public DbSet<TEntity> Set => Data as DbSet<TEntity>;

        public IEnumerable<TEntity> All(Expression<Func<TEntity, bool>> expr) => Set.Where(expr);

        public int Count(Expression<Func<TEntity, bool>> expr) => Set.Count(expr);

        public TEntity Entity(Expression<Func<TEntity, bool>> expr) => Set.FirstOrDefault(expr);

        public TEntity Entity(params object[] ids) => Set.Find(ids);

        public bool Exists(Expression<Func<TEntity, bool>> expr) => Set.Any(expr);

        public virtual TEntity Insert(TEntity entity) => Set.Add(entity);

        public virtual void Update(TEntity entity, params object[] ids) => (UnitOfWork as IEntityUnitOfWork)?.Entry(Entity(ids)).CurrentValues.SetValues(entity);

        public virtual void Delete(params object[] ids) => Set.Remove(Entity(ids));

        public virtual void OnSaveChanges(IDictionary<EntityState, IEnumerable<TEntity>> entities)
        {
        }

        public async Task<TEntity> InsertAsync(TEntity entity) => await new TaskFactory().StartNew(() => Insert(entity));
        public async Task UpdateAsync(TEntity entity, params object[] ids) => await new TaskFactory().StartNew(() => Update(entity, ids));
        public async Task DeleteAsync(params object[] ids) => await new TaskFactory().StartNew(() => Delete(ids));
        public async Task<TEntity> EntityAsync(params object[] ids) => await new TaskFactory().StartNew(() => Entity(ids));
        public async Task<TEntity> EntityAsync(Expression<Func<TEntity, bool>> expr) => await new TaskFactory().StartNew(() => Entity(expr));
    }

    public class EntityRepository<TEntity, TDTO> : EntityRepository<TEntity>, IEntityRepository<TEntity, TDTO>, IRepository<TEntity, TDTO> where TEntity : class where TDTO : class
    {
        public IMapper<TEntity, TDTO> DTOMapper { get; set; }
        public IMapper<TDTO, TEntity> EntityMapper { get; set; }

        public EntityRepository(IUnitOfWork manager) : base(manager)
        {
        }

        public TDTO ElementBuilt(Func<TEntity, bool> expr) => DTO(Expression.Lambda<Func<TEntity, bool>>(Expression.Call(expr.Method)));

        public IEnumerable<TDTO> AllBuilt() => List();

        public IEnumerable<TDTO> AllBuilt(Func<TEntity, bool> expr) => List(Expression.Lambda<Func<TEntity, bool>>(Expression.Call(expr.Method)));

        public DataSourceResult<TDTO> DataSource(int take, int skip, ICollection<Sort> sort, Filter filter, Func<TEntity, bool> expr) => DataSource(take, skip, sort, filter, Expression.Lambda<Func<TEntity, bool>>(Expression.Call(expr.Method)));

        public TDTO DTO(params object[] ids)
        {
            var entity = Entity(ids);

            return entity != null ? Builder.Build(entity).To<TDTO>() : null;
        }

        public TDTO DTO(Expression<Func<TEntity, bool>> expr)
        {
            var entity = Entity(expr);

            return entity != null ? Builder.Build(entity).To<TDTO>() : null;
        }

        public IEnumerable<TDTO> List() => Builder.BuildList(All()).ToList<TDTO>();

        public IEnumerable<TDTO> List(Expression<Func<TEntity, bool>> expr) => Builder.BuildList(All(expr)).ToList<TDTO>();

        public DataSourceResult<TDTO> DataSource(int take, int skip, ICollection<Sort> sort, Filter filter, Expression<Func<TEntity, bool>> expr) => DataSource(take, skip, sort, filter, expr, t => Builder.BuildList(t).ToList<TDTO>());

        private DataSourceResult<TDTO> DataSource(int take, int skip, ICollection<Sort> sort, Filter filter, Expression<Func<TEntity, bool>> expr, Func<IEnumerable<TEntity>, IEnumerable<TDTO>> buildFunc)
        {
            var orderBy = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(t => t.Name).FirstOrDefault();

            var res = All(expr);

            if (orderBy != null)
                res = res.OrderBy(orderBy);

            var ds = res.AsQueryable().ToDataSourceResult(take, skip, sort, filter);


            return new DataSourceResult<TDTO>
            {
                Data = buildFunc(ds.Data).ToList(),
                Total = ds.Total
            };
        }

        public TDTO Insert(TDTO dto) => Builder.Build(Insert(Builder.Build(dto).To<TEntity>())).To<TDTO>();

        public void Update(TDTO dto, params object[] ids)
        {
            var entity = Entity(ids);
            Builder.Build(dto).To(entity);
            Update(entity, ids);
        }

        /*
    member this.InsertAsync(dto : 'TDTO) = async { return this.Insert(dto) } |> Async.StartAsTask
    member this.UpdateAsync(dto : 'TDTO, ids) = async { this.Update(dto, ids) } |> Async.StartAsTask
    member this.DeleteAsync(dto : 'TDTO, [<ParamArray>] ids) = async { this.Delete(dto, ids) } |> Async.StartAsTask
    member this.ListAsync(whereExpr : Expression<Func<'TEntity, bool>>) = async { return this.List(whereExpr) } |> Async.StartAsTask
    member this.ListAsync() = async { return this.List() } |> Async.StartAsTask
    member this.DataSourceAsync(take : int, skip : int, sort : ICollection<Sort>, filter : Filter, whereFunc : Expression<Func<'TEntity, bool>>) = async { return this.DataSource(take, skip, sort, filter, whereFunc) } |> Async.StartAsTask
    member this.SingleDTOAsync([<ParamArray>] id : obj []) = async { return this.DTO(id) } |> Async.StartAsTask
    member this.SingleDTOAsync(expr : Expression<Func<'TEntity, bool>>) = async { return this.DTO(expr) } |> Async.StartAsTask
        */
    }
}