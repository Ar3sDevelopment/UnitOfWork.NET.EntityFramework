using System.Data.Entity;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models.Mapping;
using System.Runtime.CompilerServices;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Migrations;

namespace UnitOfWork.NET.EntityFramework.NUnit.Data.Models
{
	public partial class TestDbContext : DbContext
	{
		static TestDbContext()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<TestDbContext, Configuration>());
		}

		public TestDbContext()
			: base("Name=TestDbContext")
		{
		}

		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new RoleMap());
			modelBuilder.Configurations.Add(new UserMap());
			modelBuilder.Configurations.Add(new UserRoleMap());
		}
	}
}
