using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// TDesign 的扩展。
/// </summary>
public static class TDesignExtensions
{
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
    public static ValueTask<IJSModule> ImportTDesignScriptAsync(this IJSRuntime js)
        => js.ImportAsync("./_content/TDesign/tdesign-blazor.js");

    /// <summary>
    /// 引入 tdesign blazor 相关的 JS 模块对象。
    /// <para>
    /// 所有的组件模块都要放到 wwwroot/lib 文件夹下，并以 <c>tdesign-blazor-{module}.js</c> 命名。
    /// </para>
    /// </summary>
    /// <param name="js"></param>
    /// <param name="moduleJsFileName">模块的js文件名称，不要.js后缀。</param>
    /// <returns></returns>
    public static ValueTask<IJSModule> ImportTDesignModuleAsync(this IJSRuntime js, string moduleJsFileName)
        => js.ImportAsync($"./_content/TDesign/libs/tdesign-blazor-{moduleJsFileName}.js");

    /// <summary>
    /// 执行切换暗黑和浅色模式切换
    /// </summary>
    /// <param name="js"></param>
    /// <param name="isDark">是否为暗黑模式</param>
    public static async Task ChangeThemeMode(this IJSRuntime js, bool isDark = true)
    {
        var jsObject = await js.ImportTDesignScriptAsync();
        await jsObject.Module.InvokeVoidAsync(isDark ? "theme.dark" : "theme.light");
    }

    public static async ValueTask FocusAsync(this IJSRuntime js, ElementReference? inputElement,Action? focused = default)
    {
        if( inputElement is null )
        {
            return;
        }

        var tdesignScript = await js.ImportTDesignScriptAsync();
        await tdesignScript.Module.InvokeVoidAsync("tdesign.focus", inputElement);
    }
}
