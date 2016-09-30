using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using Caelan.DynamicLinq.Classes;
using UnitOfWork.NET.Interfaces;
using System.Linq;

namespace UnitOfWork.NET.EntityFramework.Interfaces
{
	public interface IEntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
	{
		new IEntityUnitOfWork UnitOfWork { get; }
		DbSet<TEntity> Set { get; }

		TEntity Entity(params object[] ids);

		TEntity Entity(Expression<Func<TEntity, bool>> expr);

		TEntity Insert(TEntity entity);

		void Update(TEntity entity, params object[] ids);

		void Delete(params object[] ids);

		void OnSaveChanges(IDictionary<EntityState, IEnumerable<TEntity>> entities);
	}

	public interface IEntityRepository<TEntity, TDTO> : IRepository<TEntity, TDTO>, IEntityRepository<TEntity> where TEntity : class, new() where TDTO : class, new()
	{
		TDTO DTO(params object[] ids);

		TDTO DTO(Expression<Func<TEntity, bool>> expr);

		IEnumerable<TDTO> List();

		IEnumerable<TDTO> List(Expression<Func<TEntity, bool>> expr);

		TDTO Insert(TDTO dto);

		void Update(TDTO dto, params object[] ids);
	}

	public interface IEntityListRepository<TSource, TDestination, TListDestination> : IEntityRepository<TSource, TDestination>, IListRepository<TSource, TDestination, TListDestination> where TSource : class, new() where TDestination : class, new() where TListDestination : class, new()
	{
		/// <summary>
		/// 
		/// </summary>
		new IEntityRepository<TSource, TListDestination> ListRepository { get; }
	}
}
