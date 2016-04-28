using System;
using System.Collections.Generic;
using UnitOfWork.NET.EntityFramework.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;
using UnitOfWork.NET.EntityFramework.NUnit.DTO;
using UnitOfWork.NET.Interfaces;

namespace UnitOfWork.NET.EntityFramework.NUnit.Repositories
{
    public class UserRepository : EntityRepository<User, UserDTO>
    {
        public UserRepository(IUnitOfWork manager) : base(manager)
        {
        }

        public IEnumerable<UserDTO> NewList()
        {
            Console.WriteLine("NewList");

            return List();
        }
    }
}
