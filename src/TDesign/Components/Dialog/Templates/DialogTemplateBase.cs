namespace TDesign.Templates;

/// <summary>
/// 表示对话框模板的基类。
/// </summary>
public abstract class DialogTemplateBase : ComponentBase
{
    /// <summary>
    /// 用于操作的对话框上下文。
    /// </summary>
    [CascadingParameter]protected DialogContext Context { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.Component<TDialog>()
            .Attribute(nameof(TDialog.HeaderContent), Context.Parameters.GetTitle())
            .Attribute(nameof(TDialog.ChildContent), Context.Parameters.GetContent())
            .Attribute(nameof(TDialog.FooterContent), BuildFooter())
            .Attribute(nameof(TDialog.Icon), Context.Parameters.GetIcon())
            .Attribute(nameof(TDialog.IconTheme), Context.Parameters.GetIconTheme())
            .Close();
    }

    /// <summary>
    /// 构建对话框的底部。
    /// </summary>
    /// <returns></returns>
    protected abstract RenderFragment? BuildFooter();
}
