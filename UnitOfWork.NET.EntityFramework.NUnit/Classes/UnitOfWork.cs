using UnitOfWork.NET.EntityFramework.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;
using UnitOfWork.NET.EntityFramework.NUnit.Repositories;
using System;

namespace UnitOfWork.NET.EntityFramework.NUnit.Classes
{
	public class TestUnitOfWork : EntityUnitOfWork<TestDbContext>
	{
		public UserRepository Users { get; set; }

		public override void AfterSaveChanges(System.Data.Entity.DbContext context)
		{
			base.AfterSaveChanges(context);

			Console.WriteLine("DbContext After SaveChanges");
		}

		public override void AfterSaveChanges(TestDbContext context)
		{
			base.AfterSaveChanges(context);

			Console.WriteLine("TestDbContext After SaveChanges");
		}

		public override void BeforeSaveChanges(System.Data.Entity.DbContext context)
		{
			base.BeforeSaveChanges(context);

			Console.WriteLine("DbContext Before SaveChanges");
		}

		public override void BeforeSaveChanges(TestDbContext context)
		{
			base.BeforeSaveChanges(context);

			Console.WriteLine("TestDbContext Before SaveChanges");
		}
	}
}
