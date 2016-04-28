using UnitOfWork.NET.EntityFramework.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;
using UnitOfWork.NET.EntityFramework.NUnit.Repositories;

namespace UnitOfWork.NET.EntityFramework.NUnit.Classes
{
    public class TestUnitOfWork : EntityUnitOfWork<TestDbContext>
    {
        public UserRepository Users { get; set; }
    }
}
