using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// 表示 popper 对象。
/// </summary>
public class Popper :DomNode, IAsyncDisposable
{
    private readonly PopperOptions? _options;   

    internal Popper(IJSObjectReference? customizeModule = null, IJSObjectReference? internalModule = null,PopperOptions? options=default) : base(customizeModule, internalModule)
    {
        this._options = options;
    }

    /// <summary>
    /// 同步更新 popper 实例。 用于低频更新。
    /// </summary>
    public ValueTask ForceUpdateAsync() => InternalModule!.InvokeVoidAsync("forceUpdate");

    /// <summary>
    /// 异步更新 popper 实例，并返回一个 promise， 用于高频更新。
    /// </summary>
    public ValueTask<PopperState?> UpdateAsync() => InternalModule!.InvokeAsync<PopperState?>("update");

    /// <summary>
    /// 更新实例的选项。
    /// </summary>
    /// <param name="options">要更新的实例。</param>
    /// <returns></returns>
    public ValueTask<PopperState?> SetOptionsAsync(PopperOptions options) => InternalModule!.InvokeAsync<PopperState?>("setOptons", options);

    /// <summary>
    /// 销毁实例。
    /// </summary>
    public ValueTask HideAsync(ElementReference? popupElement) 
        => CustomizeModule!.InvokeVoidAsync("popup.hide", InternalModule, popupElement, _options);

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if ( InternalModule is not null )
        {
            await InternalModule.DisposeAsync();
        }
    }
}
