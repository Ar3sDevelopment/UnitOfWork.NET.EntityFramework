using System;
using System.Diagnostics;
using NUnit.Framework;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;

namespace UnitOfWork.NET.EntityFramework.NUnit
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestContext()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            using (var db = new TestDbContext())
            {
                var users = db.Users;
                foreach (var user in users)
                    Console.WriteLine($"{user.Id} {user.Login}");
            }

            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        [Test]
        public void TestEntityRepository()
        {
        }

        [Test]
        public void TestDTORepository()
        {
        }

        [Test]
        public void TestCustomRepository()
        {
        }

        [Test]
        public void TestCustomUnitOfWork()
        {
        }
    }
}

