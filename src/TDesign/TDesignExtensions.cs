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
}
