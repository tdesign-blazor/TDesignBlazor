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
    [CascadingParameter(Name = "Table")]protected TTable<TItem> CascadingTable { get; set; }
    /// <summary>
    /// 获取或设置列中输出的值。
    /// </summary>
    [Parameter] public object? Value { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if(CascadingTable is null )
        {
            throw new InvalidOperationException("列必须定义在 TTable 组件中");
        }

        if ( !CascadingTable!.GetColumns().Any(m => m.Key!.Equals(Key)) )
        {
            CascadingTable.AddChildComponent(this);
        }
        base.OnInitialized();
    }
    /// <inheritdoc/>
    protected override RenderFragment? GetHeaderContent()
    {
        if ( TitleContent is null )
        {
          return  builder => builder.AddContent(0, Title ?? Value?.GetType()?.Name ?? "未命名标题");
        }
        return TitleContent;
    }
}
