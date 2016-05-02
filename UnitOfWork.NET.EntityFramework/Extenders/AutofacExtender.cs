using System;
using Autofac.Builder;
using UnitOfWork.NET.EntityFramework.Interfaces;
using UnitOfWork.NET.Interfaces;

namespace UnitOfWork.NET.EntityFramework.Extenders
{
    internal static class AutofacExtender
    {
        public static IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> AsEntityRepository<TLimit>(this IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> registration, Type entityType = null, Type dtoType = null)
        {
            var res = registration.As<IRepository>();

            if (entityType != null)
            {
                res = res.As(typeof(IRepository<>).MakeGenericType(entityType));
                res = res.As(typeof(IEntityRepository<>).MakeGenericType(entityType));

                if (dtoType != null)
                {
                    res = res.As(typeof(IRepository<,>).MakeGenericType(entityType, dtoType));
                    res = res.As(typeof(IEntityRepository<,>).MakeGenericType(entityType, dtoType));
                }
            }

            return res;
        }


        public static IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> AsEntityRepository<TLimit, TConcreteActivatorData>(this IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> registration, Type entityType = null, Type dtoType = null) where TConcreteActivatorData : IConcreteActivatorData
        {
            var res = registration.As<IRepository>();

            if (entityType != null)
            {
                res = res.As(typeof(IRepository<>).MakeGenericType(entityType));
                res = res.As(typeof(IEntityRepository<>).MakeGenericType(entityType));

                if (dtoType != null)
                {
                    res = res.As(typeof(IRepository<,>).MakeGenericType(entityType, dtoType));
                    res = res.As(typeof(IEntityRepository<,>).MakeGenericType(entityType, dtoType));
                }
            }

            return res;
        }
    }
}
