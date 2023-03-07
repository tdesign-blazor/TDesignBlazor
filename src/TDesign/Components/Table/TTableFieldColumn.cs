namespace TDesign;

/// <summary>
/// 定义可对字段绑定的列。
/// </summary>
public class TTableFieldColumn<TItem> : TTableFieldColumnBase<TItem>, IHasChildContent
{
    /// <inheritdoc/>
    internal override RenderFragment? GetColumnContent(params object[]? args)
    {
        var data = GetRowData(args);
        var value = GetValue(data);
        if ( ChildContent is null )
        {
            return builder => builder.AddContent(0, value);
        }
        return ChildContent;
    }

    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
