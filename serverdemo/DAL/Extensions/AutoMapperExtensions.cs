using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DAL.Extensions
{
    internal static class AutoMapperExtensions
    {
        public static T MapTo<T>(this object value)
        {
            return AutoMapper.Mapper.Map<T>(value);
        }

        public static IEnumerable<T> EnumerableTo<T>(this object value)
        {
            return AutoMapper.Mapper.Map<IEnumerable<T>>(value);
        }

        public static IQueryable<T> QueryableTo<T>(this object value)
        {
            return AutoMapper.Mapper.Map<IQueryable<T>>(value);
        }
    }
}
