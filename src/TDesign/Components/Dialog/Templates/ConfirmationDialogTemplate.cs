namespace TDesign.Templates;

/// <summary>
/// 拥有【确定】和【取消】按钮的确认对话框模板。
/// </summary>
public class ConfirmationDialogTemplate : DialogTemplateBase
{
    /// <inheritdoc/>
    protected override RenderFragment? BuildFooter()
        => builder => builder.Component<TButton>()
                                .Attribute(m => m.Theme, Theme.Default)
                                .Attribute(m => m.Varient, ButtonVarient.Outline)
                                .Callback(m => m.OnClick, this, e => Context.Cancel())
                                .Content("取消")
                            .Close()
                            .Component<TButton>()
                                .Attribute(m => m.Theme, Theme.Primary)
                                .Callback(m => m.OnClick, this, e => Context.Confirm(true))
                                .Content("确定")
                            .Close()

        ;
}
