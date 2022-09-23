using System.Reflection;

namespace TDesign;
/// <summary>
/// TDesign 的扩展。
/// </summary>
public static class TDesignBlazorExtensions
{
    /// <summary>
    /// 获取指定的特性。
    /// </summary>
    /// <typeparam name="TAttribute">要获取的特性。</typeparam>
    /// <param name="memberInfo"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static bool TryGetCustomAttribute<TAttribute>(this MemberInfo memberInfo, out TAttribute? attribute) where TAttribute : Attribute
    {
        attribute = memberInfo.GetCustomAttribute<TAttribute>();
        return attribute != null;
    }
}
