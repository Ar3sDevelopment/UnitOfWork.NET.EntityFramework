using System.Collections.Generic;

namespace UnitOfWork.NET.EntityFramework.NUnit.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserRoleDTO> UserRoles { get; set; }
    }
}
