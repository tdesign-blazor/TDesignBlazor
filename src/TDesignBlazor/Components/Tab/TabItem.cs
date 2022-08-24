using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;
/// <summary>
/// 选项卡的项。
/// </summary>
[ChildComponent(typeof(Tab))]
[CssClass("t-tab-panel")]
public class TabItem : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// 用于自动化获取父组件。
    /// </summary>
    [CascadingParameter] public Tab CascadingTab { get; set; }
    /// <summary>
    /// 选项卡标题。
    /// </summary>
    [Parameter] public string? Title { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 选项卡宽度，单位时 px。
    /// </summary>
    [Parameter] public int? Width { get; set; }
    /// <summary>
    /// 选项卡内容的内边距，默认时 25px。
    /// </summary>
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
