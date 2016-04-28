using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.NET.EntityFramework.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;
using UnitOfWork.NET.EntityFramework.NUnit.DTO;
using UnitOfWork.NET.Interfaces;

namespace UnitOfWork.NET.EntityFramework.NUnit.Repositories
{
    public class RoleRepository : EntityRepository<Role, RoleDTO>
    {
        public RoleRepository(IUnitOfWork manager) : base(manager)
        {
        }
    }
}
