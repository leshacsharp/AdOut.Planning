using AutoMapper;
using System;
using System.Linq;

namespace AdOut.Planning.Core.Mapping
{
    public static class MapperExtensions
    {
        public static TResult MergeInto<TResult>(this IMapper mapper, params object[] objects)
        {
            if (objects.Length < 2)
            {
                throw new ArgumentException("The array must containt 2 or more objects for merging them to the result");
            }

            var res = mapper.Map<TResult>(objects.First());
            return objects.Skip(1).Aggregate(res, (r, obj) => mapper.Map(obj, r));
        }
    }
}
