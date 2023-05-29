namespace TDesign;
/// <summary>
/// 表示可以对话框上下文。
/// </summary>
public class DialogContext : ComponentBase
{
    [CascadingParameter] DialogWrapper Wrapper { get; set; }
    /// <summary>
    /// 对话框所需要的参数。
    /// </summary>
    [Parameter] public DialogParameters Parameters { get; set; }

    /// <summary>
    /// 对话框的内容。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 取消操作并关闭对话框。
    /// </summary>
    public Task Cancel() => Wrapper.Close(DialogResult.Close());

    /// <summary>
    /// 确定操作并关闭对话框。
    /// </summary>
    /// <typeparam name="TResult">结果类型。</typeparam>
    /// <param name="result">确定操作要返回的结果。</param>
    public Task Confirm<TResult>(TResult? result = default) => Wrapper.Close(DialogResult.Ok(result));

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(this, 0, content =>
        {
            var template = Parameters.GetDialogTemplate();

            content.OpenComponent(0, template);
            content.CloseComponent();

            content.AddContent(1, ChildContent);
        });
    }


    internal void Register(TDialog dialog)
    {
        Wrapper.Set(dialog);
    }
}
