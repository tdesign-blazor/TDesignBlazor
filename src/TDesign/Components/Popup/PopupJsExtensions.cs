using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;
public static class PopupJsExtensions
{
    public static async ValueTask<PopperInstance> InvokePopupAsync(this IJSRuntime js,ElementReference reference,ElementReference popper,PopupOptions options)
    {
        //var path = "./_content/TDesign/tdesign-blazor.js";
        //var module = await js.InvokeAsync<IJSInProcessObjectReference>("import", path);

        //var instance = await module.InvokeAsync<IJSObjectReference>("createPopper", new object[] { reference, popper, "top" });


        //return new(instance, module);

        //var module = await js.Import(path);
        //var instance = module.createPopper<PopperInstance>(reference, popper, "top");
        //return instance;


        var objRef = DotNetObjectReference.Create(options);
        var popperWrapper = await js.InvokeAsync<IJSInProcessObjectReference>("import", "./_content/TDesign/tdesign-blazor.js");
        var jSInstance = await popperWrapper.InvokeAsync<IJSObjectReference>("createPopper", reference, popper, options, objRef);
        return new(jSInstance, objRef, popperWrapper);
    }
}
