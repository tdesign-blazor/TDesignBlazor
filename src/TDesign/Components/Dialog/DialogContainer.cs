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
        if ( Configuration is not null  )
        {
            builder.CreateCascadingComponent(Configuration.Data, 0, content =>
            {
                content.OpenComponent(0, Configuration.ComponentType);
                content.CloseComponent();
            }, "DialogData");

            //using var dialog = builder.Component<TDialog>();
            //var componentType = Configuration.ComponentType;
            //var component= Activator.CreateInstance(componentType);

            //dialog.Attribute(nameof(TDialog.HeaderText), Configuration.Title);

            //RenderFragmentConversion(dialog, component, "HeaderContent");
            //RenderFragmentConversion(dialog, component, "FooterContent");

            //dialog
            //    .Attribute(nameof(TDialog.Modeless), Configuration.Modeless)
            //    .Callback(nameof(TDialog.OnClosed), HtmlHelper.Instance.Callback().Create(this, Close))
            //    .Ref<TDialog>(component => DialogRef = component)
            //    .ChildContent(content =>
            //    {
            //        content.CreateCascadingComponent(DialogRef, 0, body =>
            //        {
            //            body.CreateCascadingComponent(Configuration.Data, 0, content =>
            //            {
            //                body.OpenComponent(0, Configuration.ComponentType);
            //                body.CloseComponent();
            //            });

            //        }, isFixed: true);
            //    });
        }
    }

    private static void RenderFragmentConversion(IFluentAttributeBuilder dialog, object? component, string parameterName)
    {
        if ( dialog is null )
        {
            throw new ArgumentNullException(nameof(dialog));
        }
        if(component is null )
        {
            throw new ArgumentNullException(nameof(component));
        }

        if ( string.IsNullOrEmpty(parameterName) )
        {
            throw new ArgumentException($"'{nameof(parameterName)}' cannot be null or empty.", nameof(parameterName));
        }

        var type = component.GetType();

        var parameter = type.GetProperty(parameterName);
        if ( parameter is not null && parameter.CanRead )
        {
            var fragment = (RenderFragment?)parameter.GetValue(component);//TODO throw target not match
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
