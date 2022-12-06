using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
[CssClass("t-dialog__ctx")]
public class TDialog : TDesignComponentBase, IHasChildContent, IHasOnActive
{
    /// <summary>
    /// 设置非模态对话框。
    /// </summary>
    [Parameter][BooleanCssClass("t-dialog__ctx--modelss", "t-dialog__ctx--fixed")] public bool Modeless { get; set; }
    /// <summary>
    /// 设置屏幕居中显示。
    /// </summary>
    [Parameter] public bool Center { get; set; }

    /// <summary>
    /// 设置一个当对话框显示或关闭时执行的回调方法。
    /// </summary>
    public EventCallback<bool> OnActive { get; set; }
    /// <summary>
    /// 设置显示对话框。
    /// </summary>
    [Parameter] public bool Active { get; set; }
    /// <summary>
    /// 设置对话框顶部的内容。
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    /// <summary>
    /// 设置对话框消息的内容。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置对话框底部的内容。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", attributes: new { @class = "t-dialog__mask" }, condition: !Modeless);

        builder.CreateElement(sequence + 1, "div", wrap =>
        {
            wrap.CreateElement(0, "div", position =>
            {
                position.CreateElement(0, "div", dialog =>
                {
                    //Header
                    dialog.CreateElement(0, "div", header => {
                        //Header Content
                        header.CreateElement(0, "div", HeaderContent, new { @class = "t-dialog__header-content" });

                        //Close TIcon
                        dialog.CreateElement(1, "span", close =>
                        {
                            close.CreateComponent<TIcon>(0, attributes: new { Name = IconName.Close });
                        }, new
                        {
                            @class = "t-dialog__close",
                            onclick = HtmlHelper.Event.Create(this, () => this.Activate(false))
                        });

                    }, new { @class = "t-dialog__header" });

                    //Content
                    dialog.CreateElement(2, "div", ChildContent, new { @class = "t-dialog__body__icon" }, ChildContent is not null);

                    //Footer
                    dialog.CreateElement(3, "div", FooterContent, new { @class = "t-dialog__footer" }, FooterContent is not null);
                }, new
                {
                    @class = HtmlHelper.Class.Append("t-dialog")
                                                            .Append("t-dialog--default")
                });
            }, new { @class = HtmlHelper.Class.Append("t-dialog__position").Append("t-dialog--center", Center, "t-dialog--top") });
        }, new { @class = "t-dialog__wrap" });
    }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append("display:none", !Active);
    }
}