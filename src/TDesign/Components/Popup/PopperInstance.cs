using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;
public class PopperInstance:IDisposable
{
    private readonly IJSObjectReference _jSInstance;
    private readonly DotNetObjectReference<PopupOptions> _objRef;
    private readonly IJSInProcessObjectReference _popperWrapper;

    public PopperInstance(IJSObjectReference jSInstance, DotNetObjectReference<PopupOptions> objRef, IJSInProcessObjectReference popperWrapper)
    {
        this._jSInstance = jSInstance;
        this._objRef = objRef;
        this._popperWrapper = popperWrapper;
    }
    public State State
    {
        get { return _popperWrapper.Invoke<State>("getStateOfInstance", _jSInstance); }
    }
    public async Task ForceUpdate() => await _jSInstance.InvokeVoidAsync("forceUpdate");
    public async Task<State> Update() => await _popperWrapper.InvokeAsync<State>("updateOnInstance", _jSInstance);
    public async Task<State> SetOptions(PopupOptions options) => await _popperWrapper.InvokeAsync<State>("setOptionsOnInstance", _jSInstance, options, _objRef);
    public async Task Destroy() => await _jSInstance.InvokeVoidAsync("destroy");

    public void Dispose()
    {
        _objRef?.Dispose();
    }
}
