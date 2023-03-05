namespace TDesign;

/// <summary>
/// 表示 <see cref="TTable{TItem}"/> 组件中行事件的参数。
/// </summary>
/// <typeparam name="TItem">项的类型。</typeparam>
public class TTableRowItemEventArgs<TItem> : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TTableRowItemEventArgs{TItem}"/> class.
    /// </summary>
    /// <param name="item">行中对应的数据。。</param>
    /// <param name="rowIndex">行索引。</param>
    public TTableRowItemEventArgs(TItem? item, int rowIndex)
    {
        Item = item;
        RowIndex = rowIndex;
    }

    /// <summary>
    /// 获取行的数据。
    /// </summary>
    public TItem? Item { get; }
    /// <summary>
    /// 获取行的索引位置。
    /// </summary>
    public int RowIndex { get; }

    /// <summary>
    /// 获取一个布尔值，表示指定的行是否被选择。
    /// </summary>
    public bool IsSelected => Item is not null;
}
