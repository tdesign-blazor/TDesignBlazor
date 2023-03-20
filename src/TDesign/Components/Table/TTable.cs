namespace TDesign;

/// <summary>
/// 数据表格组件。
/// </summary>
[CssClass("t-table")]
[CascadingTypeParameter(nameof(TItem))]
public partial class TTable<TItem> : TDesignComponentBase
{
    #region 参数
    /// <summary>
    /// 设置行索引的键。默认是当前行。
    /// </summary>
    [Parameter] public Func<TItem, object> ItemKey { get; set; } = x => x!;
    /// <summary>
    /// 设置表格的数据源。
    /// </summary>
    [Parameter,EditorRequired] public DataSource<TItem> Data { get; set; }
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
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 加载中状态。值为 true 会显示默认加载中样式。
    /// </summary>
    [Parameter] public bool Loading { get; set; }
    /// <summary>
    /// 设置当表格数据是空时显示的自定义内容。
    /// </summary>
    [Parameter] public RenderFragment? EmptyContent { get; set; }
    /// <summary>
    /// 开启分页模式。
    /// </summary>
    [Parameter] public bool Pagination { get; set; }
    /// <summary>
    /// 表示当前的页码。
    /// </summary>
    [Parameter] public int PageIndex { get; set; } = 1;
    /// <summary>
    /// 当页码变更时执行的方法。
    /// </summary>
    [Parameter] public EventCallback<int> PageIndexChanged { get; set; }
    /// <summary>
    /// 设置每页呈现的数据量。
    /// </summary>
    [Parameter] public int PageSize { get; set; } = 10;
    /// <summary>
    /// 当数据量变更时执行的方法。
    /// </summary>
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }

    /// <summary>
    /// 设置表尾的自定义内容。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }

    #region 选中行
    /// <summary>
    /// 设置当行被点击选择后的回调方法。
    /// </summary>
    [Parameter] public EventCallback<TTableRowItemEventArgs<TItem>> OnRowSelected { get; set; }
    #endregion

    #region 可编辑/操作
    /// <summary>
    /// 设置一个点击【编辑】操作时触发的回调方法。
    /// </summary>
    [Parameter] public EventCallback<TTableRowItemEventArgs<TItem>> OnRowEditing { get; set; }
    /// <summary>
    /// 设置一个点击【提交】操作时触发的回调方法。
    /// </summary>
    [Parameter] public EventCallback<TTableRowItemEventArgs<TItem>> OnRowEdited { get; set; }
    #endregion
    #endregion

    #region Internal
    /// <summary>
    /// 总页数。
    /// </summary>
    internal int TotalCount { get; set; } = 1;
    /// <summary>
    /// 设置记录总数变更的回调。
    /// </summary>
    internal EventCallback<int> TotalCountChanged { get; set; }

    /// <summary>
    /// 表示当数据加载完成。
    /// </summary>
    internal bool DataLoadedComplete { get; set; }

    /// <summary>
    /// 已加载的数据。内部使用。
    /// </summary>
    internal List<(TableRowDataType type, TItem? data)> TableData { get; set; } = new();


    /// <summary>
    /// 获取或设置选择行的集合。
    /// </summary>
    internal List<TTableRowItemEventArgs<TItem>> SelectedRows { get; set; } = new();

    /// <summary>
    /// 获取一个布尔值，表示行仅可以被单选。
    /// </summary>
    internal bool IsSingleSelection { get; set; }

    /// <summary>
    /// 当前表格是否处于编辑状态。
    /// </summary>
    internal Dictionary<int, bool> EditableState { get; set; } = new();

    #endregion
}


/// <summary>
/// 行的数据类型。
/// </summary>
internal enum TableRowDataType
{
    /// <summary>
    /// 数据类型未知
    /// </summary>
    Unknow,
    /// <summary>
    /// 正常的数据行。
    /// </summary>
    Data,
    /// <summary>
    /// 展开行。
    /// </summary>
    Expand
}