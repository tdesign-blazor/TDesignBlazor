using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// 表示 popper 对象。
/// </summary>
public class Popper : IAsyncDisposable
{
    private readonly IJSObjectReference? _popper;
    private readonly PopperOptions? _options;
    

    /// <summary>
    /// Initializes a new instance of the <see cref="Popper"/> class.
    /// </summary>
    internal Popper(IJSObjectReference popper, PopperOptions options)
    {
        this._popper = popper;
        this._options = options;
    }

    /// <summary>
    /// 同步更新 popper 实例。 用于低频更新。
    /// </summary>
    public ValueTask ForceUpdateAsync()
    {
        return _popper!.InvokeVoidAsync("forceUpdate");
    }

    /// <summary>
    /// 异步更新 popper 实例，并返回一个 promise， 用于高频更新。
    /// </summary>
    public ValueTask<PopperState?> UpdateAsync()
    {
        return _popper!.InvokeAsync<PopperState?>("update");
    }

    /// <summary>
    /// 更新实例的选项。
    /// </summary>
    /// <param name="options">要更新的实例。</param>
    /// <returns></returns>
    public ValueTask<PopperState?> SetOptionsAsync(PopperOptions options)
    {
        return _popper!.InvokeAsync<PopperState?>("setOptons", options);
    }

    /// <summary>
    /// 销毁实例。
    /// </summary>
    public ValueTask DestroyAsync()
    {
        return _popper!.InvokeVoidAsync("destroy");
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if ( _popper is not null )
        {
            await _popper.DisposeAsync();
            await DisposeAsync();
        }
    }
}
