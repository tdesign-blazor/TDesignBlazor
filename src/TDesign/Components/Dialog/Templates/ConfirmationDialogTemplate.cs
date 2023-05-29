namespace TDesign.Templates;

/// <summary>
/// 拥有【确定】和【取消】按钮的确认对话框模板。
/// </summary>
public class ConfirmationDialogTemplate : DialogTemplateBase
{
    /// <inheritdoc/>
    protected override RenderFragment? BuildFooter()
        => builder => builder.Component<TButton>()
                                .Attribute(nameof(TButton.Theme), Theme.Default)
                                .Attribute(nameof(TButton.Varient),ButtonVarient.Outline)
                                .Attribute(nameof(TButton.OnClick), HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, Context.Cancel))
                                .ChildContent("取消")
                            .Close()
                            .Component<TButton>()
                                .Attribute(nameof(TButton.Theme), Theme.Primary)
                                .Attribute(nameof(TButton.OnClick), HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, e => Context.Confirm(true)))
                                .ChildContent("确定")
                            .Close()

        ;
}
