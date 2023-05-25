namespace TDesign;

public static class DialogServiceExtensions
{
    public static Task Open(this IDialogService dialogService, RenderFragment? fragment, DialogConfiguration? configuration = default)
    {
        return dialogService.Open<OKDialogTemplate>(configuration);
    }
}
