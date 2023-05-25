using TDesign.Abstractions;

namespace TDesign;

/// <summary>
/// 用于动态渲染 <see cref="TDialog"/> 组件的容器。
/// </summary>
internal class DialogContainer:ComponentBase,IContainerComonent,IDisposable
{
    [Parameter]public RenderFragment? ChildContent { get; set; }
    [Inject]IDialogService DialogService { get; set; }

    DialogConfiguration Configuration { get; set; }

    bool Active { get; set; }

    TDialog? DialogRef;
    protected override void OnInitialized()
    {
        base.OnInitialized();
        DialogService.OnOpening += DialogService_OnOpening;
    }

    private Task DialogService_OnOpening(DialogConfiguration arg)
    {
        Configuration = arg;
        Active = true;
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if ( Configuration is not null && Active )
        {
            using var dialog = builder.Component<TDialog>();
            var componentType = Configuration.ComponentType;

            dialog.Attribute(nameof(TDialog.HeaderText), Configuration.Title);

            RenderFragmentConversion(dialog, componentType, "HeaderContent");
            RenderFragmentConversion(dialog, componentType, "FooterContent");

            dialog
                .Attribute(nameof(TDialog.Modeless), Configuration.Modeless)
                .Callback(nameof(TDialog.OnClosed), HtmlHelper.Instance.Callback().Create(this, Close))
                .Ref<TDialog>(component => DialogRef = component)
                .ChildContent(content =>
                {
                    content.CreateCascadingComponent(DialogRef, 0, body =>
                    {
                        body.OpenComponent(0,Configuration.ComponentType);
                        body.CloseComponent();

                    }, isFixed: true);
                });
        }
    }

    private static void RenderFragmentConversion(IFluentAttributeBuilder dialog, Type componentType,string parameterName)
    {
        var parameter = componentType.GetProperty(parameterName);
        if ( parameter is not null && parameter.CanRead )
        {
            var fragment = (RenderFragment?)parameter.GetValue(componentType);//TODO throw target not match
            dialog.Attribute(parameterName, fragment);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (DialogRef is not null )
        {
            await DialogRef.Open();
        }
    }

    public void Dispose()
    {
        DialogService.OnOpening -= DialogService_OnOpening;
    }

    Task Close()
    {
        Active = false;
        StateHasChanged();
        return Task.CompletedTask;
    }
}
