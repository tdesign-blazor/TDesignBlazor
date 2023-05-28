namespace TDesign;

public static class DialogServiceExtensions
{
    public static Task<IDialogReference> Open(this IDialogService dialogService, RenderFragment? fragment, DialogParameters? parameters = default)
    {
        parameters ??= new DialogParameters();
        parameters["Content"] = fragment;

        return dialogService.Open<OKDialog>(parameters);
    }

    public static Task<IDialogReference> Open(this IDialogService dialogService,string? content=default,DialogParameters? parameters = default)
    {
        return Open(dialogService, builder => builder.AddContent(0, content), parameters);
    }
}
