using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TDesign
{
    /// <summary>
    /// 
    /// </summary>
    [HtmlTag("div")]
    [ParentComponent]
    [CssClass("t-anchor")]
    public class TAnchor : BlazorComponentBase, IHasChildContent, IHasOnSwitch
    {
        /// <summary>
        /// 
        /// </summary>
        public TAnchor()
        {
            SwitchIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        [Parameter] public RenderFragment? ChildContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EventCallback<int?> OnSwitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? SwitchIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Inject] public new IJSRuntime? JS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            BuildLine(builder, sequence + 1);
            builder.AddContent(sequence + 2, ChildContent);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="builder"></param>
        private void BuildLine(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateElement(sequence + 1, "div", line =>
            {

                line.CreateElement(sequence + 2, "div", wrapper =>
                {

                    wrapper.CreateElement(sequence + 3, "div", attributes: new
                    {

                        @class = "t-anchor__line-cursor",


                    });

                }, new { @class = "t-anchor__line-cursor-wrapper", style = $"top: {SwitchIndex * 24}px; height: 24px; opacity: 1;" });

            }, new { @class = "t-anchor__line" });

            JS?.InvokeVoidAsync("anchorOnScroll", objRef);
        }

        private DotNetObjectReference<TAnchor>? objRef;

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            objRef = DotNetObjectReference.Create(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            objRef?.Dispose();
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">scrollTop 位置</param>
        /// <returns></returns>
        [JSInvokable]
        public async Task OnScrollAnchorChangeAsync(int index)
        {
            for (int i = 0; i < ChildComponents.Count; i++)
            {
                if (ChildComponents[i] is TAnchorItem item)
                {
                        var start = item.OffsetTop - 22 - item.OffsetHeight;
                        var end = item.OffsetTop;

                        if (start <= index && index <= end)
                        {
                            SwitchIndex = item.Index;
                            item.SetActive(true);
                            await item.Refresh();
                        }
                        else
                        {
                            item.SetActive(false);
                            await item.Refresh();
                        }
                }
            }
            await this.Refresh();
        }
    }
}
