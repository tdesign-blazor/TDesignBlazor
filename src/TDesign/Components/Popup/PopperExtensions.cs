using Microsoft.JSInterop;

namespace TDesign;
public static class PopperExtensions
{
    public static async ValueTask<PopperInstance> InvokePopupAsync(this IJSRuntime js, object selector, ElementReference popupRef, PopperOptions options)
    {
        var optionRef = DotNetObjectReference.Create(options);
        var jsObject = await js.ImportScriptAsync();
        var popper = await jsObject.InvokeAsync<IJSObjectReference>("popup.create", selector, popupRef, options, optionRef);


        //var jsObject = await js.ImportAsync("_content/TDesign/js/popper/popper.js");
        //var popper = jsObject.createPopper<IJSObjectReference>(objRef, popupRef, options);

        return new(popper, options, jsObject);
    }
}
