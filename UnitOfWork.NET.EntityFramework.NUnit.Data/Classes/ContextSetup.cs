using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Migrations;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;

namespace UnitOfWork.NET.EntityFramework.NUnit.Data.Classes
{
    public static class ContextSetup
    {
        public static void InitDatabase()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<TestDbContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<TestDbContext, Configuration>());
        }
    }
}
