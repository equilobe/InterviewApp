using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.Common.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType)
        {
            return baseType.Assembly.GetTypes()
                            .Where(p => baseType.IsAssignableFrom(p));
        }
    }
}
