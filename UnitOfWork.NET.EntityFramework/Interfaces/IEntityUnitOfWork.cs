using System;
using System.Collections.Generic;
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
        DbEntityEntry Entry<TEntity>(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        void Transaction(Action<IEntityUnitOfWork> body);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        bool TransactionSaveChanges(Action<IEntityUnitOfWork> body);

        /// <summary>
        /// 
        /// </summary>
        IEntityRepository<TEntity> EntityRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        IEntityRepository<TEntity, TDTO> EntityRepository<TEntity, TDTO>() where TEntity : class where TDTO : class;
    }
}
