using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using UnitOfWork.NET.EntityFramework.Interfaces;

namespace UnitOfWork.NET.EntityFramework.Classes
{
    public class EntityUnitOfWork : NET.Classes.UnitOfWork
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

        public DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class => _dbContext.Entry(entity);
        [Obsolete("Use Set instead")]
        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class => _dbContext.Set<TEntity>();
        public DbSet<TEntity> Set<TEntity>() where TEntity : class => _dbContext.Set<TEntity>();

        public IEntityRepository<TEntity> EntityRepository<TEntity>() where TEntity : class => Repository<TEntity>() as IEntityRepository<TEntity>;

        public IEntityRepository<TEntity> EntityRepository<TEntity, TDTO>() where TEntity : class where TDTO : class => Repository<TEntity, TDTO>() as IEntityRepository<TEntity, TDTO>;

        /*    
    member uow.SaveChanges() = 
        context.ChangeTracker.DetectChanges()
        let entries = context.ChangeTracker.Entries()
        let entriesGroup = 
            entries
            |> Seq.filter (fun t -> t.State <> EntityState.Unchanged && t.State <> EntityState.Detached)
            |> Array.ofSeq
            |> Array.groupBy (fun t -> ObjectContext.GetObjectType(t.Entity.GetType()))
        
        let entitiesGroup = (entriesGroup |> Array.map (fun (t, e) -> (t, e.ToList().GroupBy((fun i -> i.State), (fun (i : DbEntityEntry) -> i.Entity)).ToList()))).ToList()
        context |> uow.BeforeSaveChanges
        let res = context.SaveChanges()
        for item in entitiesGroup do
            let (entityType, entitiesByState) = item
            let mHelper = uow.GetType().GetMethod("CallOnSaveChanges", BindingFlags.NonPublic ||| BindingFlags.Instance)
            mHelper.MakeGenericMethod([| entityType |]).Invoke(uow, [| entitiesByState.ToDictionary((fun t -> t.Key), (fun (t : IGrouping<EntityState, obj>) -> t.AsEnumerable())) |]) |> ignore
        context |> uow.AfterSaveChanges
        res
    
    member this.SaveChangesAsync() = async { return this.SaveChanges() } |> Async.StartAsTask
    abstract BeforeSaveChanges : context:DbContext -> unit
    override uow.BeforeSaveChanges context = ()
    abstract AfterSaveChanges : context:DbContext -> unit
    override uow.AfterSaveChanges context = ()
    
    member private uow.CallOnSaveChanges<'TEntity when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null>(entitiesObj : Dictionary<EntityState, IEnumerable<obj>>) = 
        let entities = entitiesObj.ToDictionary((fun t -> t.Key), (fun (t : KeyValuePair<EntityState, IEnumerable<obj>>) -> t.Value.Cast<'TEntity>()))
        uow.EntityRepository<'TEntity>().OnSaveChanges(entities)
    
    member this.Transaction(body : Action<IEntityUnitOfWork>) = 
        use transaction = context.Database.BeginTransaction()
        try 
            this |> body.Invoke
            transaction.Commit()
        with _ -> transaction.Rollback()
    
    member this.TransactionSaveChanges(body : Action<IEntityUnitOfWork>) = 
        use transaction = context.Database.BeginTransaction()
        try 
            this |> body.Invoke
            let res = this.SaveChanges() <> 0
            transaction.Commit()
            res
        with _ -> 
            transaction.Rollback()
            false
        */
    }

    public class EntityUnitOfWork<TContext> : EntityUnitOfWork where TContext : DbContext, new()
    {
        public EntityUnitOfWork() : base(new TContext(), true)
        {
        }

        /*
    member uow.BeforeSaveChanges(context : DbContext) = base.BeforeSaveChanges context
    member uow.AfterSaveChanges(context : DbContext) = base.AfterSaveChanges context
    abstract BeforeSaveChanges : context:'TContext -> unit
    override uow.BeforeSaveChanges(context : 'TContext) = uow.BeforeSaveChanges(context :> DbContext)
    abstract AfterSaveChanges : context:'TContext -> unit
    override uow.AfterSaveChanges(context : 'TContext) = uow.AfterSaveChanges(context :> DbContext)
        */
    }
}
