namespace TDesign.Templates;

/// <summary>
/// 只有一个【确定】按钮的消息提示对话框。
/// </summary>
public class MessageDialogTemplate : DialogTemplateBase
{
   /// <inheritdoc/>
   protected override RenderFragment? BuildFooter()
        => builder => builder.Component<TButton>()
                            .Attribute(nameof(TButton.Theme), Theme.Primary)
                            .Attribute(nameof(TButton.OnClick), HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, Context.Confirm))
                            .ChildContent("确定")
                            .Close();
}
