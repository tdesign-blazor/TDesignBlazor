namespace TDesign;

/// <summary>
/// 表示可以输出数据的表格列的基类。必须在 <see cref="TTable{TItem}"/> 组件中使用。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
public abstract class TTableFieldColumnBase<TItem> : TTableColumnBase
{
    /// <summary>
    /// 级联的 <see cref="TTable{TItem}"/> 组件。
    /// </summary>
    [CascadingParameter(Name = "GenericTable")]protected TTable<TItem> CascadingGenericTable { get; set; }
    /// <summary>
    /// 设置数据源的字段名称。
    /// </summary>
    [Parameter][EditorRequired] public string? Field { get; set; }

    /// <summary>
    /// 获取具备字段值的内容。
    /// </summary>
    /// <param name="rowIndex">行索引。</param>
    /// <param name="columnIndex">列索引。</param>
    /// <param name="rowData">每一行的数据。</param>
    internal virtual RenderFragment? GetFieldValueContent(in int rowIndex, in int columnIndex, in TItem? rowData)
    {
        var value = GetValue(rowData);
        return builder => builder.AddContent(0, value);
    }

    /// <inheritdoc/>
    internal override RenderFragment? GetColumnContent(in int rowIndex, in int columnIndex)
    {
        throw new NotSupportedException($"请使用{nameof(GetFieldValueContent)}方法");
    }
    /// <summary>
    /// 获取指定数据的值。
    /// </summary>
    /// <param name="item">要获取的数据对象。</param>
    /// <returns>值或 null。</returns>
    /// <exception cref="TDesignComponentException">字段与数据的属性不匹配。</exception>
    protected object? GetValue(in TItem? item)
    {
        if ( item is null || string.IsNullOrEmpty(Field) )
        {
            return default;
        }

        var property = item.GetType().GetProperty(Field);
        if ( property is null )
        {
            throw new TDesignComponentException(this, $"没有在{item.GetType().Name}找到字段{Field}");
        }

        return property.GetValue(item);
    }
}
