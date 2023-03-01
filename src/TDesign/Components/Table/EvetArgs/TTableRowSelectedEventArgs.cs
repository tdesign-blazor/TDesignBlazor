namespace TDesign;

/// <summary>
/// 表示 <see cref="TTable{TItem}"/> 组件中行选中事件的参数。
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class TTableRowSelectedEventArgs<TItem>:EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TTableRowSelectedEventArgs{TItem}"/> class.
    /// </summary>
    /// <param name="item">选中的项。</param>
    /// <param name="index">选中的行索引。</param>
    public TTableRowSelectedEventArgs(TItem item, int index)
    {
        Item = item;
        Index = index;
    }

    /// <summary>
    /// 获取选中行的数据。
    /// </summary>
    public TItem Item { get; }
    /// <summary>
    /// 获取选中行的索引位置。
    /// </summary>
    public int Index { get; }
}
