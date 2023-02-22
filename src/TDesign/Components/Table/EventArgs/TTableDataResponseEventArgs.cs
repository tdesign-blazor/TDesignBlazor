namespace TDesign;

public class TTableDataResponseEventArgs<TItem>:EventArgs
{
    public TTableDataResponseEventArgs(IEnumerable<TItem> data,int count)
    {
        Data = data;
        Count = count;
    }

    public IEnumerable<TItem> Data { get; }
    public int Count { get; }
}
