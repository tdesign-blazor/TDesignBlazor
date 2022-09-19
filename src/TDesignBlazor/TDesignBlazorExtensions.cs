using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TDesignBlazor;
public static class TDesignBlazorExtensions
{
    public static bool TryGetCustomAttribute<TAttribute>(this MemberInfo memberInfo, out TAttribute? attribute) where TAttribute : Attribute
    {
        attribute = memberInfo.GetCustomAttribute<TAttribute>();
        return attribute != null;
    }
}
