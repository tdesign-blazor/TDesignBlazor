namespace TDesign;
public record struct DialogResult
{
    public bool Closed { get; internal set; }
    public object? Data { get; internal set; }

    public static DialogResult Close() => new() { Closed = true };    
    public static DialogResult Ok<T>(T result) => new() { Data = result };
}

public interface IDialogReference
{
    Task<DialogResult> Result { get; }
    Guid Id { get; }
}

internal class DialogReference : IDialogReference
{
    TaskCompletionSource<DialogResult> _result = new();
    public DialogReference()
    {
        
    }

    public Task<DialogResult> Result => _result.Task;

    public Guid Id => Guid.NewGuid();

    public bool SetResult(DialogResult result)
    {
        return _result.TrySetResult(result);
    }
}
