using ClassBuilder.Classes;
using UnitOfWork.NET.EntityFramework.NUnit.Data.Models;
using UnitOfWork.NET.EntityFramework.NUnit.DTO;

namespace UnitOfWork.NET.EntityFramework.NUnit.Mappers
{
    public class UserDTOMapper : DefaultMapper<User, UserDTO>
    {
        public override UserDTO CustomMap(User source, UserDTO destination)
        {
            var res = base.CustomMap(source, destination);

            res.Id = source.Id;
            res.Login = source.Login;
            res.Password = source.Password;

            return res;
        }
    }
}
