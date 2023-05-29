namespace TDesign;
/// <summary>
/// 对话框是一种临时窗口，通常在不想中断整体任务流程，但又需要为用户展示信息或获得用户响应时，在页面中打开一个对话框承载相应的信息及操作。
/// </summary>
public class TDialog : ComponentBase
{
    /// <summary>
    /// 上下文。
    /// </summary>
    [CascadingParameter]DialogContext Context { get; set; }
    /// <summary>
    /// 对话框消息的任意内容。
    /// </summary>
    [Parameter]public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 对话框标题的任意内容。
    /// </summary>
    [Parameter]public RenderFragment? HeaderContent { get; set; }
    /// <summary>
    /// 对话框用于操作的任意内容。
    /// </summary>
    [Parameter]public RenderFragment? FooterContent { get; set; }
    /// <summary>
    /// 设置标题的图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 设置图标的主题。
    /// </summary>
    [Parameter] public Theme? IconTheme { get; set; }

    /// <summary>
    /// 是否显示 x 关闭图标。
    /// </summary>
    [Parameter] public bool Closable { get; set; } = true;
    /// <summary>
    /// 设置非模态对话框。
    /// </summary>
    [Parameter] public bool Modeless { get; set; }
    /// <summary>
    /// 设置屏幕居中显示。
    /// </summary>
    [Parameter] public bool Center { get; set; }
    /// <summary>
    /// 阻止点击遮罩层关闭对话框功能。
    /// </summary>
    [Parameter]public bool PreventMaskToClose { get; set; }

    protected override void OnInitialized()
    {
        Context?.Register(this);
    }
}