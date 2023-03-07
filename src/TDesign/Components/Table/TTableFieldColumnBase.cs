using System.Diagnostics.CodeAnalysis;

namespace TDesign;

/// <summary>
/// 表示可以输出数据的表格列的基类。必须在 <see cref="TTable{TItem}"/> 组件中使用。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
public abstract class TTableFieldColumnBase<TItem> : TTableColumnBase
{
    [CascadingParameter(Name ="GenericTable")]internal TTable<TItem> CascadingGenericTable { get; set; }
    /// <summary>
    /// 设置数据源的字段名称。
    /// </summary>
    [Parameter][EditorRequired] public string? Field { get; set; }

    /// <summary>
    /// 从自定义参数中获取行数据，第一个参数必须是 <typeparamref name="TItem"/> 类型。
    /// </summary>
    /// <param name="args">上下文的参数数组。</param>
    /// <returns>行数据。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="args"/> 是 null。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="args"/> 长度是0。</exception>
    /// <exception cref="ArgumentException">第一个参数不是 <typeparamref name="TItem"/> 类型。</exception>
    protected TItem GetRowData(object[]? args)
    {
        if ( args is null )
        {
            throw new ArgumentNullException(nameof(args));
        }
        if(args.Length == 0 )
        {
            throw new ArgumentOutOfRangeException(nameof(args),$"{args}的参数不能是空的");
        }
        var rowData= (TItem?)args[0];
        return rowData ?? throw new ArgumentException($"{args}第一个参数的值必须是{nameof(TItem)}类型");
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

    /// <summary>
    /// 获取指定的值所在的行索引。
    /// </summary>
    /// <param name="value">字段的值。</param>
    /// <returns>若找到数据则返回其索引，否则返回 <c>-1</c>。</returns>
    protected int FindRowIndex(object? value)
    {
        var index = CascadingGenericTable.TableData.FindIndex(m =>
        {
            if ( m.data is null )
            {
                return false;
            }
            return m.data.GetType().GetProperty(Field!).GetValue(m.data).Equals(value);
        });
        return index;
    }
}
