namespace TDesign;
/// <summary>
/// 对话框是一种临时窗口，通常在不想中断整体任务流程，但又需要为用户展示信息或获得用户响应时，在页面中打开一个对话框承载相应的信息及操作。
/// </summary>
public class TDialog : ComponentBase
{
    [CascadingParameter]DialogContext Context { get; set; }
    [Parameter]public RenderFragment? ChildContent { get; set; }
    [Parameter]public RenderFragment? HeaderContent { get; set; }
    [Parameter]public RenderFragment? FooterContent { get; set; }

    protected override void OnInitialized()
    {
        Context?.Register(this);
    }
}