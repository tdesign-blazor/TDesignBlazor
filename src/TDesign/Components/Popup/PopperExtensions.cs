using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// Popup 的 JS 扩展。
/// </summary>
public static class PopperExtensions
{
    /// <summary>
    /// 调用 popup 组件
    /// </summary>
    /// <param name="js"></param>
    /// <param name="selectorRef">触发 popup 组件的元素引用。</param>
    /// <param name="popupRef">Popup组件元素的引用。</param>
    /// <param name="options">Popup的配置。</param>
    public static async ValueTask<Popper> InvokePopupAsync(this IJSRuntime js, ElementReference selectorRef, ElementReference popupRef, PopperOptions options)
    {
        var jsObject = await js.ImportScriptAsync();
        var popperObject = await jsObject.InvokeAsync<IJSObjectReference>("popup.create", selectorRef, popupRef, options, DotNetObjectReference.Create(options));

        return new (popperObject, options);
    }
}
