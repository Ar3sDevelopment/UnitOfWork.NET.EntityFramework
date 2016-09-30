using System;
using UnitOfWork.NET.EntityFramework.Classes;
using UnitOfWork.NET.Interfaces;
namespace UnitOfWork.NET.EntityFramework.NUnit.Repositories
{
	public abstract class CustomRepository<TEntity, TDTO> : EntityRepository<TEntity, TDTO> where TEntity : class, new() where TDTO : class, new()
	{
		protected CustomRepository(IUnitOfWork manager) : base(manager)
		{
		}
	}
}
