using ComponentBuilder;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    public static class JsRuntimeExtensions
    {
        public static async Task ScrollToHash(this IJSRuntime js, string hash)
        {
            var element = await js.InvokeAsync<Element>("getDomInfo", hash);

            var options = new
            {
                Top = element.AbsoluteTop - 80,
                Left = 0,
                Behavior = "smooth"
            };

            await js.InvokeVoidAsync("window.scrollTo", options);
        }
    }
}
