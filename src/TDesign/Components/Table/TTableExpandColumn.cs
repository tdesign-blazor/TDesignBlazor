namespace TDesign;

/// <summary>
/// 定义可展开详情行的列。
/// </summary>
[CssClass("t-table__expandable-icon-cell")]
public class TTableExpandColumn : TTableColumnBase,IHasChildContent
{
    /// <summary>
    /// 设置要展开的图标。
    /// </summary>
    [Parameter] public object? ExpandIcon { get; set; } = IconName.ChevronRightCircle;
    /// <summary>
    /// 设置要收起的图标。
    /// </summary>
    [Parameter] public object? CollapseIcon { get; set; } = IconName.ChevronDownCircle;
    /// <summary>
    /// 设置展开的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    internal override RenderFragment? GetColumnContent(in int rowIndex, in int columnIndex)
    {
        return builder => builder.Fluent().Span("t-table__expand-box")
                                            .Class($"t-table__row--{(Expanded?"expanded":"collapsed")}")
                                            .Class("t-positive-rotate-90", Expanded)
                                            .Content(content => content.CreateComponent<TIcon>(0, attributes:new{ Name=Expanded?CollapseIcon:ExpandIcon}))
                                        .Close();
    }

    internal override RenderFragment? GetHeaderContent() => builder => builder.AddContent(0, "展开/收起");

    /// <summary>
    /// 获取展开行的 UI 内容。
    /// </summary>
    internal RenderFragment? GetExpandedRow()
    => builder => builder.Fluent().Element("tr", "t-table__expanded-row",Expanded)
                                    .Content(td => td.CreateElement(0, "td", content =>
                                    {
                                        content.Fluent().Div("t-table__expanded-row-inner")
                                                            .Content(div => div.Fluent().Div("t-table__row-full-element")
                                                            .Content(ChildContent).Close())
                                                        .Close();
                                    }, new {colspan=base.CascadingTable.ChildComponents.Count}))
                                    .Close();

    /// <summary>
    /// 行是否展开。
    /// </summary>
    internal bool Expanded { get; set; }
}
