using System;
using System.Linq;
using System.Collections.Generic;
using UnitOfWork.NET.EntityFramework.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;
using UnitOfWork.NET.EntityFramework.NUnit.DTO;
using UnitOfWork.NET.Interfaces;
using System.Linq.Dynamic;

namespace UnitOfWork.NET.EntityFramework.NUnit.Repositories
{
	public class UserRepository : EntityRepository<User, UserDTO, UserListDTO>
	{
		public UserRepository(IUnitOfWork manager) : base(manager)
		{
		}

		public IEnumerable<UserDTO> NewList()
		{
			Console.WriteLine("NewList");

			return List();
		}

		public override void OnSaveChanges(IDictionary<System.Data.Entity.EntityState, IEnumerable<User>> entities)
		{
			base.OnSaveChanges(entities);

			Console.WriteLine("On User Save Changes");
			Console.WriteLine("Users saving: " + string.Join(", ", entities.SelectMany(t => t.Value.Select(u => u.Login))));
		}
	}
}
