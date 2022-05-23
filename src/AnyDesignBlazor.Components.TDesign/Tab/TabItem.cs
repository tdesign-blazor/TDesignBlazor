using ComponentBuilder;
using ComponentBuilder.Parameters;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AnyDesignBlazor.Components.TDesign;
[ChildComponent(typeof(Tab))]
[CssClass("t-tab-panel")]
public class TabItem : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter] public Tab CascadingTab { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public int? Width { get; set; }
    [Parameter] public int Padding { get; set; } = 25;

    internal int Index { get; private set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Index = CascadingTab.ChildComponents.Count - 1;
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (CascadingTab.SwitchIndex.HasValue && CascadingTab.SwitchIndex.Value == Index)
        {
            builder.CreateElement(sequence, "p", content =>
            {
                content.AddContent(0, ChildContent);
            }, new { style = $"padding:{Padding}px" });
        }
    }
}
