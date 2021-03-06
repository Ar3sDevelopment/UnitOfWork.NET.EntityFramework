﻿using System;
using System.Diagnostics;
using NUnit.Framework;
using UnitOfWork.NET.EntityFramework.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;
using UnitOfWork.NET.EntityFramework.NUnit.DTO;
using UnitOfWork.NET.EntityFramework.NUnit.Repositories;

namespace UnitOfWork.NET.EntityFramework.NUnit
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestContext()
        {
            var stopwatch = Stopwatch.StartNew();

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
            var stopwatch = Stopwatch.StartNew();

            using (var uow = new EntityUnitOfWork<TestDbContext>())
            {
                var users = uow.Repository<User>().All();
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);

                foreach (var user in users)
                    Console.WriteLine($"{user.Id} {user.Login}");

                var entity = new User
                {
                    Login = "test",
                    Password = "test"
                };

                entity = uow.Repository<User>().Insert(entity);
                uow.SaveChanges();
                Assert.AreNotEqual(entity.Id, 0);
                Console.WriteLine(entity.Id);
                entity.Password = "test2";
                uow.Repository<User>().Update(entity, entity.Id);
                uow.SaveChanges();
                Assert.AreEqual(entity.Password, uow.Repository<User>().Entity(entity.Id).Password);
                uow.Repository<User>().Delete(entity.Id);
                uow.SaveChanges();
                Assert.IsNull(uow.Repository<User>().Entity(entity.Id));
            }
        }

        [Test]
        public void TestDTORepository()
        {
            var stopwatch = Stopwatch.StartNew();

            using (var uow = new EntityUnitOfWork<TestDbContext>())
            {
                var users = uow.Repository<User, UserDTO>().List();
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);

                foreach (var user in users)
                    Console.WriteLine($"{user.Id} {user.Login}");

                var dto = new UserDTO
                {
                    Login = "test",
                    Password = "test"
                };

                dto = uow.Repository<User, UserDTO>().Insert(dto);
                uow.SaveChanges();
                var login = dto.Login;
                dto = uow.Repository<User, UserDTO>().DTO(d => d.Login == login);
                Assert.IsNotNull(dto);
                Assert.AreNotEqual(dto.Id, 0);
                Console.WriteLine(dto.Id);
                dto.Password = "test2";
                uow.Repository<User, UserDTO>().Update(dto, dto.Id);
                uow.SaveChanges();
                Assert.AreEqual(dto.Password, uow.Repository<User, UserDTO>().DTO(dto.Id).Password);
                uow.Repository<User, UserDTO>().Delete(dto.Id);
                uow.SaveChanges();
                Assert.IsNull(uow.Repository<User, UserDTO>().DTO(dto.Id));
            }
        }

        [Test]
        public void TestCustomRepository()
        {
            var stopwatch = Stopwatch.StartNew();

            using (var uow = new EntityUnitOfWork<TestDbContext>())
            {
                var users = uow.CustomRepository<UserRepository>().NewList();
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
                foreach (var user in users)
                    Console.WriteLine($"{user.Id} {user.Login}");
                var dto = new UserDTO
                {
                    Login = "test",
                    Password = "test"
                };

                dto = uow.CustomRepository<UserRepository>().Insert(dto);
                uow.SaveChanges();
                var login = dto.Login;
                dto = uow.CustomRepository<UserRepository>().DTO(d => d.Login == login);
                Assert.IsNotNull(dto);
                Assert.AreNotEqual(dto.Id, 0);
                Console.WriteLine(dto.Id);
                dto.Password = "test2";
                uow.CustomRepository<UserRepository>().Update(dto, dto.Id);
                uow.SaveChanges();
                Assert.AreEqual(dto.Password, uow.CustomRepository<UserRepository>().DTO(dto.Id).Password);
                uow.CustomRepository<UserRepository>().Delete(dto.Id);
                uow.SaveChanges();
                Assert.IsNull(uow.CustomRepository<UserRepository>().DTO(dto.Id));
            }
        }

        [Test]
        public void TestCustomUnitOfWork()
        {
            var stopwatch = Stopwatch.StartNew();

            using (var uow = new TestUnitOfWork())
            {
                var users = uow.Users.NewList();
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
                foreach (var user in users)
                    Console.WriteLine($"{user.Id} {user.Login}");
                var dto = new UserDTO
                {
                    Login = "test",
                    Password = "test"
                };
                dto = uow.Users.Insert(dto);
                uow.SaveChanges();
                var login = dto.Login;
                dto = uow.Users.DTO(d => d.Login == login);
                Assert.IsNotNull(dto);
                Assert.AreNotEqual(dto.Id, 0);
                Console.WriteLine(dto.Id);
                dto.Password = "test2";
                uow.Users.Update(dto, dto.Id);
                uow.SaveChanges();
                Assert.AreEqual(dto.Password, uow.Users.DTO(dto.Id).Password);
                uow.Users.Delete(dto.Id);
                uow.SaveChanges();
                Assert.IsNull(uow.Users.DTO(dto.Id));
            }
        }

        [Test]
        public void TestReflection()
        {
            using (var uow = new TestUnitOfWork())
            {
                Console.WriteLine(uow.UserRolesRegistered);

                Assert.IsNotNull(uow.Repository<User, UserDTO>(), "uow.Users != null by reflection");
                Assert.IsNotNull(uow.Repository<Role, RoleDTO>(), "uow.Roles != null by reflection");
                Assert.IsNotNull(uow.Repository<UserRole, UserRoleDTO>(), "uow.UserRoles != null by reflection");

                Assert.IsNotNull(uow.Users, "uow.Users != null");
                Assert.IsNotNull(uow.Roles, "uow.Roles != null");
                Assert.IsNotNull(uow.UserRoles, "uow.UserRoles != null");
            }
        }
    }
}

