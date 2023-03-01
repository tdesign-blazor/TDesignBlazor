namespace TDesign;

/// <summary>
/// 表示可以输出数据的表格列。必须在 <see cref="TTable{TItem}"/> 组件中使用。
/// </summary>
public class TTableFieldColumn<TItem> : TTableFieldColumnBase<TItem>, IHasChildContent
{
    /// <inheritdoc/>
    protected override void AfterSetParameters(ParameterView parameters)
    {
        base.AfterSetParameters(parameters);

        ChildContent ??= builder => builder.AddContent(0, Value);
    }

    /// <inheritdoc/>
    protected override RenderFragment? GetColumnContent() => ChildContent;

    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
