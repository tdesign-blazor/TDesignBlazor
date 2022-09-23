namespace TDesignBlazor;

/// <summary>
/// 表示可以输出数据的表格列。必须在 <see cref="Table{TItem}"/> 组件中使用。
/// </summary>
public class FieldColumn<TItem> : TableColumnBase
{
    [CascadingParameter(Name = "Table")] protected Table<TItem>? CascadingTable { get; set; }
    /// <summary>
    /// 获取或设置列中输出的值。
    /// </summary>
    [Parameter] public object? Value { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        ArgumentNullException.ThrowIfNull(CascadingTable, nameof(CascadingTable));

        await CascadingTable.AddChildComponent(this);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override RenderFragment? GetColumnContent() => builder => builder.AddContent(0, Value);
}
