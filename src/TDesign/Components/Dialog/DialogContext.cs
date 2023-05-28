namespace TDesign;
public class DialogContext : ComponentBase
{
    [CascadingParameter] DialogWrapper Wrapper { get; set; }
    [Parameter] public DialogParameters Parameters { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public Task Cancel() => Wrapper.Close(DialogResult.Close());

    public Task Ok<TResult>(TResult? result = default) => Wrapper.Close(DialogResult.Ok(result));

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
        Wrapper.Set(dialog.HeaderContent, dialog.ChildContent, dialog.FooterContent);
    }
}
