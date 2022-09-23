using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 只具备一些基本功能的数据表格。
/// </summary>
[CssClass("t-table")]
[CascadingTypeParameter(nameof(TItem))]
public class Table<TItem> : BlazorComponentBase
{
    /// <summary>
    /// 设置数据源。
    /// </summary>
    [Parameter] public IEnumerable<TItem>? Data { get; set; }
    /// <summary>
    /// 设置是否为自动列宽，默认是固定的。
    /// </summary>
    [Parameter] public bool AutoWidth { get; set; }
    /// <summary>
    /// 设置行具备条纹间隔的样式。
    /// </summary>
    [Parameter][CssClass("t-table--striped")] public bool Striped { get; set; }
    /// <summary>
    /// 设置列具备边框的样式。
    /// </summary>
    [Parameter][CssClass("t-table--bordered")] public bool Bordered { get; set; }
    /// <summary>
    /// 设置行具备鼠标悬浮时高亮样式。
    /// </summary>
    [Parameter][CssClass("t-table--hoverable")] public bool Hoverable { get; set; }
    /// <summary>
    /// 设置表格的尺寸。
    /// </summary>
    [Parameter][CssClass("t-is-")] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 设置固定列头。
    /// </summary>
    [Parameter] public bool FixedHeader { get; set; }
    /// <summary>
    /// 设置固定底部。
    /// </summary>
    [Parameter] public bool FixedFooter { get; set; }
    /// <summary>
    /// 当固定头部或底部时，表格内容部分的固定高度，单位是 px。
    /// </summary>
    [Parameter] public int? FixedHeight { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment<TItem>? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        base.OnInitialized();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(this, 0, base.BuildRenderTree, "Table");
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildTable(builder, sequence + 1);
        BuildPagination(builder, sequence + 2);
    }

    /// <summary>
    /// 构建表格。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildTable(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            builder.CreateElement(0, "table", table =>
            {
                BuildTableHeader(table, 0);
                BuildTableBody(table, 1);
                BuildTableFooter(table, 2);
            },
            new
            {
                @class = HtmlHelper.CreateCssBuilder().Append(AutoWidth, "t-table--layout-auto", "t-table--layout-fixed")
            });
        }, new
        {
            @class = "t-table__content",
            style = HtmlHelper.CreateStyleBuilder().Append($"max-height:{FixedHeight}px", FixedHeight.HasValue)
        });
    }

    /// <summary>
    /// 构建分页。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildPagination(RenderTreeBuilder builder, int sequence)
    {

    }
    /// <summary>
    /// 构建 theader 部分。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildTableHeader(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence + 1, "thead", content =>
        {
            content.CreateComponent<TableRow>(0, tr =>
            {
                tr.CreateCascadingComponent(true, sequence, ChildContent?.Invoke(default), "IsHeader");
            });

        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-table__header")
                                                    .Append("t-table__header--fixed", FixedHeader)
        });
    }
    /// <summary>
    /// 构建 tbody 部分。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildTableBody(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "tbody", content =>
        {
            var index = 0;
            foreach (var item in Data!)
            {
                var key = index;
                content.CreateComponent<TableRow>(index + 1, tr =>
                {
                    tr.AddContent(0, ChildContent!.Invoke(item));
                }, appendFunc: (b, s) =>
                {
                    b.SetKey(key);
                    return s;
                });

                index++;
            }

        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-table__body")
        });
    }
    /// <summary>
    /// 构建 tfooter 部分。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildTableFooter(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "tfoot", content =>
        {

        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-table__footer")
                                                    .Append("t-table__footer--fixed", FixedFooter)
        });
    }
}
