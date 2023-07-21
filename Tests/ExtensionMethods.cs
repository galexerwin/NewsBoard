using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class ExtensionMethods
    {
        public static bool IsNullOrEmpty(this IList<string> list)
        {
            return list == null || list.Count() < 1;
        }
        public static bool IsNullOrEmpty<TVal>(this IDictionary<string, TVal> dictionary)
        {
            return dictionary == null || dictionary.Count() < 1;
        }
    }
}
