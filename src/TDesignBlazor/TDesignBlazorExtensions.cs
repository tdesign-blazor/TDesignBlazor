using System.Reflection;
using System.Runtime.CompilerServices;

namespace TDesignBlazor;
/// <summary>
/// TDesignBlazor 的扩展。
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

    /// <summary>
    /// 获取状态对应的图标名称。
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="mapping">状态和图标的映射关系。</param>
    /// <returns>图标枚举。</returns>
    public static IconName? GetStatusTIconName(this Status status, Func<Status, IconName>? mapping = default)
    {
        if (mapping is null)
        {
            return status switch
            {
                Status.Default => IconName.InfoCircle,
                Status.Success => IconName.CheckCircle,
                Status.Error => IconName.CloseCircle,
                Status.Warning => IconName.ErrorCircle,
                _ => default
            };
        }
        return mapping(status);
    }
}
