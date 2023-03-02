namespace TDesign;

/// <summary>
/// 表示可以输出数据的表格列。必须在 <see cref="TTable{TItem}"/> 组件中使用。
/// </summary>
public class TTableFieldColumn<TItem> : TTableColumnBase
{
    /// <summary>
    /// 级联的 <see cref="TTable{TItem}"/> 组件。
    /// </summary>
    [CascadingParameter(Name = "Table")]TTable<TItem>? CascadingTable { get; set; }
    /// <summary>
    /// 获取或设置列中输出的值。
    /// </summary>
    [Parameter] public object? Value { get; set; }

    protected override void OnInitialized()
    {
        if ( !CascadingTable!.ChildComponents.OfType<TTableColumnBase>().Any(m => m.Key!.Equals(Key)) )
        {
            CascadingTable.AddChildComponent(this);
        }

        base.OnInitialized();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override RenderFragment? GetColumnContent() => builder => builder.AddContent(0, Value);
}
