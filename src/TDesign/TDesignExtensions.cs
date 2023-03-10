using Microsoft.JSInterop;
using System.Reflection;

namespace TDesign;
/// <summary>
/// TDesign 的扩展。
/// </summary>
public static class TDesignExtensions
{
    public static bool TryGetCustomAttribute<TAttribute>(this MemberInfo? memberInfo, out TAttribute attribute) where TAttribute : Attribute
    {
        attribute = memberInfo?.GetCustomAttribute<TAttribute>();
        return attribute is not null;
    }
    /// <summary>
    /// 获取状态对应的图标名称。
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="mapping">状态和图标的映射关系。</param>
    /// <returns>图标枚举。</returns>
    public static IconName? GetStatusIconName(this Status status, Func<Status, IconName>? mapping = default)
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

    /// <summary>
    /// 引入 tdesign-blazor.js 的 JS 对象。
    /// </summary>
    /// <param name="js"></param>
    /// <returns></returns>
    public static ValueTask<IJSObjectReference> ImportTDesignScriptAsync(this IJSRuntime js)
        => js.InvokeAsync<IJSObjectReference>("import", "./_content/TDesign/tdesign-blazor.js");

    /// <summary>
    /// 执行切换暗黑和浅色模式切换
    /// </summary>
    /// <param name="js"></param>
    /// <param name="isDark">是否为暗黑模式</param>
    public static async Task ChangeThemeMode(this IJSRuntime js, bool isDark = true)
    {
        var jsObject = await js.ImportTDesignScriptAsync();
        await jsObject.InvokeVoidAsync(isDark ? "theme.dark" : "theme.light");
    }
}
