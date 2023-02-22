namespace TDesign;

public class TTableDataRequestEventArgs:EventArgs
{
    internal TTableDataRequestEventArgs(int page,int size)
    {
        Page = page;
        Size = size;
    }

    public int Page { get; }
    public int Size { get; }
}
