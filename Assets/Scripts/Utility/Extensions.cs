using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Utility
{
    public static class Extensions
    {
        public static IEnumerable<T> DeepClone<T>(this IEnumerable<T> items) where T : class, ICloneable 
            => items.Select(i => i.Clone() as T);
        public static T CloneCasted<T>(this T item) where T : class, ICloneable 
            => item.Clone() as T;
    }
}
