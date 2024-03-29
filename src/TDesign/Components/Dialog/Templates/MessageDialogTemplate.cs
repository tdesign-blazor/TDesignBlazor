﻿namespace TDesign.Templates;

/// <summary>
/// 只有一个【确定】按钮的消息提示对话框。
/// </summary>
public class MessageDialogTemplate : DialogTemplateBase
{
   /// <inheritdoc/>
   protected override RenderFragment? BuildFooter()
        => builder => builder.Component<TButton>()
                            .Attribute(m=>m.Theme, Theme.Primary)
                            .Attribute(m=>m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs?>(this, Context.Confirm))
                            .Content("确定")
                            .Close();
}
