namespace TDesign;

/// <summary>
/// 定义可对字段绑定的列。
/// </summary>
public class TTableFieldColumn<TItem> : TTableFieldColumnBase<TItem>, IHasChildContent
{
    internal override RenderFragment? GetFieldValueContent(in int rowIndex,in int columnIndex, in TItem? rowData)
    {
        if ( ChildContent is null )
        {
           return base.GetFieldValueContent(rowIndex, columnIndex, rowData);
        }
        return ChildContent;
    }

    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
