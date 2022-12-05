using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// 表示 popper 对象。
/// </summary>
public class PopperInstance : IDisposable
{
    private readonly IJSObjectReference _popper;
    private readonly PopperOptions _options;
    private readonly IJSObjectReference _js;
    private PopperState state;

    /// <summary>
    /// 初始化 <see cref="PopperInstance"/> 类的新实例。
    /// </summary>
    /// <param name="popper">调用 js 返回的 popper 对象。</param>
    /// <param name="options">popper 配置。</param>
    /// <param name="js">引入的 js 模块对象。</param>
    public PopperInstance(IJSObjectReference popper, PopperOptions options, IJSObjectReference js)
    {
        this._popper = popper;
        this._options = options;
        this._js = js;
    }
    /// <summary>
    /// 获取状态。
    /// </summary>
    public ValueTask<PopperState> GetStateAsync() => _js.InvokeAsync<PopperState>("getState", _popper);

    /// <summary>
    /// 同步更新 popper 实例。 用于低频更新。
    /// </summary>
    public ValueTask ForceUpdateAsync() => _popper.InvokeVoidAsync("forceUpdate");
    /// <summary>
    /// 异步更新 popper 实例，并返回一个 promise， 用于高频更新。
    /// </summary>
    public ValueTask<PopperState> UpdateAsync() => _popper.InvokeAsync<PopperState>("update");
    /// <summary>
    /// 更新实例的选项。
    /// </summary>
    /// <param name="options">要更新的实例。</param>
    /// <returns></returns>
    public ValueTask<PopperState> SetOptionsAsync(PopperOptions options) => _popper.InvokeAsync<PopperState>("setOptons", options);
    /// <summary>
    /// 销毁实例。
    /// </summary>
    public ValueTask DestroyAsync() => _popper.InvokeVoidAsync("destroy");

    public void Dispose()
    {
        _popper?.DisposeAsync();
        _js?.DisposeAsync();
    }
}
