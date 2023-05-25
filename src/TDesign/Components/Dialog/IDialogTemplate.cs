namespace TDesign;

public interface IDialogTemplate
{
}

public abstract class DialogTemplateBase :ComponentBase, IDialogTemplate
{
    [CascadingParameter]protected TDialog? CascadingDialog { get; set; }

    protected Task Close()
    {
        if ( CascadingDialog != null )
        {
            return CascadingDialog.Close();
        }
        return Task.CompletedTask;
    }
}
