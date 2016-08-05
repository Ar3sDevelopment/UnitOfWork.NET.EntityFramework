using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.NET.Interfaces;

namespace UnitOfWork.NET.EntityFramework.Interfaces
{
    public interface IEntityUnitOfWork : IUnitOfWork
    {
        void BeforeSaveChanges(DbContext context);

        /// <summary>
        /// 
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        void AfterSaveChanges(DbContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        bool Transaction(Action<IEntityUnitOfWork> body);
        
        bool Transaction(IsolationLevel isolationLevel, Action<IEntityUnitOfWork> body);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        bool TransactionSaveChanges(Action<IEntityUnitOfWork> body);
        
        bool TransactionSaveChanges(IsolationLevel isolationLevel, Action<IEntityUnitOfWork> body);

        /// <summary>
        /// 
        /// </summary>
        new IEntityRepository<TEntity> Repository<TEntity>() where TEntity : class, new();

        /// <summary>
        /// 
        /// </summary>
        new IEntityRepository<TEntity, TDTO> Repository<TEntity, TDTO>() where TEntity : class, new() where TDTO : class, new();
    }
}
