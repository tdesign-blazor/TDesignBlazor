﻿namespace TDesign;

/// <summary>
/// 只具备一些基本功能的数据表格。
/// </summary>
[CssClass("t-table")]
[CascadingTypeParameter(nameof(TItem))]
public partial class TTable<TItem> : TDesignComponentBase
{
    #region 参数
    /// <summary>
    /// 设置表格的数据源。如果时需要动态数据，则使用 <see cref="DataQuery"/> 参数。
    /// <para>
    /// 如果 <see cref="Data"/> 和 <see cref="DataQuery"/> 均有值，则优先使用 <see cref="DataQuery"/> 参数。
    /// </para>
    /// <para>
    /// 如果 <see cref="Data"/> 和 <see cref="DataQuery"/> 都是 <c>null</c>，则抛出异常。
    /// </para>
    /// </summary>
    [Parameter]public IEnumerable<TItem>? Data { get; set; }
    /// <summary>
    /// 设置一个读取数据源的委托。该参数优先于 <see cref="Data"/> 参数应用。
    /// </summary>
    [Parameter] public Func<TTableDataRequestEventArgs, Task<TTableDataResponseEventArgs<TItem>>>? DataQuery { get; set; }
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

    /// <summary>
    /// 加载中状态。值为 true 会显示默认加载中样式。
    /// </summary>
    [Parameter] public bool Loading { get; set; }
    /// <summary>
    /// 设置当表格数据是空时显示的自定义内容。
    /// </summary>
    [Parameter]public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// 表示当前的页码。
    /// </summary>
    [Parameter] public int PageIndex { get; set; } = 1;
    /// <summary>
    /// 当页码变更时执行的方法。
    /// </summary>
    [Parameter]public EventCallback<int> PageIndexChanged { get; set; }
    /// <summary>
    /// 设置每页呈现的数据量。
    /// </summary>
    [Parameter] public int PageSize { get; set; } = 10;
    /// <summary>
    /// 当数据量变更时执行的方法。
    /// </summary>
    [Parameter]public EventCallback<int> PageSizeChanged { get; set; }
    #endregion

    #region Internal
    /// <summary>
    /// 总页数。
    /// </summary>
    internal int TotalCount { get; set; } = 1;
    internal EventCallback<int> TotalCountChanged { get; set; }

    /// <summary>
    /// 已加载的数据源。
    /// </summary>
    internal IEnumerable<TItem> DataLoaded { get; set; } = Enumerable.Empty<TItem>();
    #endregion
}
