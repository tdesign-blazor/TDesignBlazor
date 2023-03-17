using System.Linq.Expressions;

namespace TDesign;

/// <summary>
/// 表示可以输出数据的表格列的基类。必须在 <see cref="TTable{TItem}"/> 组件中使用。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
public class TTableFieldColumn<TItem,TField> : TTableColumnBase<TItem>,IHasChildContent<TItem>
{
    private Expression<Func<TItem, TField>>? _lastAssignedField;
    private Func<TItem, string?>? _cellTextFunc;

    /// <summary>
    /// 设置数据源的字段名称。
    /// </summary>
    [Parameter, EditorRequired] public Expression<Func<TItem, TField>> Field { get; set; } = default;

    /// <summary>
    /// 设置字段输出的格式。要符合 <see cref="IFormattable"/> 的定义。
    /// </summary>
    [Parameter] public string? Format { get; set; }
    /// <inheritdoc/>
    [Parameter]public RenderFragment<TItem>? ChildContent { get; set; }

    protected override void AfterSetParameters(ParameterView parameters)
    {
        base.AfterSetParameters(parameters);

        Header ??= (Field.Body as MemberExpression)?.Member?.Name;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if ( _lastAssignedField != Field )
        {
            _lastAssignedField = Field;

            var compiledFieldExpression = Field.Compile();
            if ( !string.IsNullOrEmpty(Format) )
            {
                var nullableType = Nullable.GetUnderlyingType(typeof(TField)); //获取可为空的字段类型。
                if ( !typeof(IFormattable).IsAssignableFrom(nullableType ?? typeof(TField)) )
                {
                    throw new TDesignComponentException(this, $"提供了 {nameof(Format)} 参数的值，但是字段 {typeof(TField)} 没有实现 {nameof(IFormattable)} 接口");
                }
                _cellTextFunc = item => ((IFormattable?)compiledFieldExpression(item!))?.ToString(Format, default);
            }
            else
            {

                _cellTextFunc = item => compiledFieldExpression(item!)?.ToString();
            }
        }

        ChildContent ??= value => builder => builder.AddContent(0, _cellTextFunc!(value));
    }

    protected internal override RenderFragment? GetColumnContent(TItem item)
    => builder => builder.AddContent(0, ChildContent, item);

    ///// <summary>
    ///// 从自定义参数中获取行数据，第一个参数必须是 <typeparamref name="TItem"/> 类型。
    ///// </summary>
    ///// <param name="args">上下文的参数数组。</param>
    ///// <returns>行数据。</returns>
    ///// <exception cref="ArgumentNullException"><paramref name="args"/> 是 null。</exception>
    ///// <exception cref="ArgumentOutOfRangeException"><paramref name="args"/> 长度是0。</exception>
    ///// <exception cref="ArgumentException">第一个参数不是 <typeparamref name="TItem"/> 类型。</exception>
    //protected TItem GetRowData(object[]? args)
    //{
    //    if ( args is null )
    //    {
    //        throw new ArgumentNullException(nameof(args));
    //    }
    //    if(args.Length == 0 )
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(args),$"{args}的参数不能是空的");
    //    }
    //    var rowData= (TItem?)args[0];
    //    return rowData ?? throw new ArgumentException($"{args}第一个参数的值必须是{nameof(TItem)}类型");
    //}

    ///// <summary>
    ///// 获取指定数据的值。
    ///// </summary>
    ///// <param name="item">要获取的数据对象。</param>
    ///// <returns>值或 null。</returns>
    ///// <exception cref="TDesignComponentException">字段与数据的属性不匹配。</exception>
    //protected object? GetValue(in TItem? item)
    //{
    //    if ( item is null || string.IsNullOrEmpty(Field) )
    //    {
    //        return default;
    //    }

    //    var property = item.GetType().GetProperty(Field);
    //    if ( property is null )
    //    {
    //        throw new TDesignComponentException(this, $"没有在{item.GetType().Name}找到字段{Field}");
    //    }

    //    return property.GetValue(item);
    //}

    ///// <summary>
    ///// 获取当前字段的指定值所在的行索引。
    ///// </summary>
    ///// <param name="value">字段的值。</param>
    ///// <returns>若找到数据则返回其索引，否则返回 <c>-1</c>。</returns>
    //protected int FindRowIndex(object? value) => FindRowIndex(Field, value);

    ///// <summary>
    ///// 获取指定字段的值所在的行索引。
    ///// </summary>
    ///// <param name="field">字段。</param>
    ///// <param name="value">字段的值。</param>
    ///// <returns>若找到数据则返回其索引，否则返回 <c>-1</c>。</returns>
    //protected int FindRowIndex(string? field, object? value)
    //{
    //    if ( string.IsNullOrWhiteSpace(field) )
    //    {
    //        throw new ArgumentException($"{nameof(field)} 不能是 null 或空白字符串");
    //    }

    //    var index = CascadingGenericTable.TableData.FindIndex(row =>
    //    {
    //        if ( row.data is null )
    //        {
    //            return false;
    //        }
    //        var property = row.data.GetType()!.GetProperty(field);
    //        if(property is null )
    //        {
    //            return false;
    //        }
    //        return property!.GetValue(row.data).Equals(value);
    //    });
    //    return index;
    //}
}
