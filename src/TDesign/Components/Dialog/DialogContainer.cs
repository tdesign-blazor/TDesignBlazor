using TDesign.Abstractions;

namespace TDesign;

/// <summary>
/// 用于动态渲染 <see cref="TDialog"/> 组件的容器。
/// </summary>
internal class DialogContainer:ComponentBase,IContainerComonent,IDisposable
{
    [Parameter]public RenderFragment? ChildContent { get; set; }
    [Inject]IDialogService DialogService { get; set; }

    Dictionary<Guid,DialogParameters> DialogCollection = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        DialogService.OnOpening += DialogService_OnOpening;
        DialogService.OnClosing += DialogService_OnClosing;
    }

    private void DialogService_OnClosing(Guid id, DialogResult result)
    {
        Close(id);
    }

    private void DialogService_OnOpening(Guid id, DialogParameters? parameters)
    {
        //Thread.Sleep(400);
        DialogCollection.Add(id, parameters);
        InvokeAsync(StateHasChanged);
    }


    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        //if (!Open || Parameters is null)
        //{
        //    return;
        //}

        foreach (var item in DialogCollection)
        {
            builder.CreateCascadingComponent(this, 0, content =>
            {
                content.CreateCascadingComponent(item.Value, 0, inner =>
                {
                    inner.CreateComponent<DialogWrapper>(0, attributes: new { Id = item.Key, Parameters = item.Value }, key: item.Key);
                });

            });
        }
    }
    
    internal void Close(Guid id)
    {
        Thread.Sleep(150);//延迟等动画效果完成
        if (DialogCollection.Remove(id))
        {
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        DialogService.OnOpening -= DialogService_OnOpening;
        DialogService.OnClosing -= DialogService_OnClosing;
    }
}
