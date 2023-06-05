using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 气泡确认框通常用于不会造成严重后果的二次确认场景，其会在点击元素上弹出浮层进行提示确认。气泡确认框没有蒙层，点击确认框以外的区域即可关闭。
/// </summary>
[CssClass("t-popup t-poconfirm")]
public class TPopConfirm:TPopup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TPopConfirm"/> class.
    /// </summary>
    public TPopConfirm()
    {
        Arrow = true;
        Trigger = PopupTrigger.Click;
    }

    /// <summary>
    /// 设置主题颜色。
    /// </summary>
    [Parameter][CssClass("t-popconfirm__popup--")] public Theme Theme { get; set; } = Theme.Default;

    /// <summary>
    /// 设置当点击【确认】按钮时的回调。
    /// </summary>
    [Parameter]public EventCallback<PopConfirmEventArgs> OnConfirm { get; set; }
    /// <summary>
    /// 设置确认按钮的文本。
    /// </summary>
    [Parameter] public string? ConfirmBtnText { get; set; } = "确定";
    /// <summary>
    /// 设置确认按钮的变体样式。
    /// </summary>
    [Parameter] public ButtonVarient ConfirmBtnVarient { get; set; } = ButtonVarient.Base;
    /// <summary>
    /// 设置确认按钮的主题。
    /// </summary>
    [Parameter] public Theme ConfirmBtnTheme { get; set; } = Theme.Primary;

    /// <summary>
    /// 设置取消按钮的文本。
    /// </summary>
    [Parameter] public string? CancelBtnText { get; set; } = "取消";
    /// <summary>
    /// 设置点击【取消】按钮时的回调。
    /// </summary>
    [Parameter] public EventCallback<PopConfirmEventArgs> OnCancel { get; set; }
    /// <summary>
    /// 设置取消按钮的变体样式。
    /// </summary>
    [Parameter] public ButtonVarient CancelBtnVarient { get; set; } = ButtonVarient.Outline;
    /// <summary>
    /// 设置取消按钮的主题。
    /// </summary>
    [Parameter] public Theme CancelBtnTheme { get; set; } = Theme.Default;
    /// <summary>
    /// 设置确认和取消按钮的尺寸。
    /// </summary>
    [Parameter] public Size BtnSize { get; set; } = Size.Small;
    /// <summary>
    /// 设置确认框的内容。
    /// </summary>
    [Parameter]public RenderFragment? ConfirmContent { get; set; }
    /// <summary>
    /// 设置自定义图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; } = IconName.InfoCircleFilled;

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        this.PopupContent = BuildPopConfirmContent;
        this.ConfirmContent ??= builder => builder.AddContent(0, Content);
    }

    /// <summary>
    /// 构建确认框的内容。
    /// </summary>
    /// <param name="builder"></param>
    void BuildPopConfirmContent(RenderTreeBuilder builder)
    => builder
            .Div("t-popconfirm__content")
                .Content(content => content
                            .Div("t-popconfirm__body")
                                .Content(icon => icon
                                                    .Component<TIcon>()
                                                        .Attribute(new
                                                        {
                                                            Name = Icon,
                                                            AdditionalClass = $"t-popconfirm__icon--{Theme.Value}"
                                                        })
                                                    .Close()
                                                    .Div("t-popconfirm__inner")
                                                        .Content(ConfirmContent)
                                                    .Close())
                            .Close()
                            .Div("t-popconfirm__buttons").Content(buttons =>
                            {
                                BuildButton(buttons, 0, CancelBtnText, OnCancel, CancelBtnVarient, CancelBtnTheme, "t-popconfirm__cancel");
                                BuildButton(buttons, 0, ConfirmBtnText, OnConfirm, ConfirmBtnVarient, ConfirmBtnTheme, "t-popconfirm__confirm");

                            })
                            .Close())
                .Close();
                            
                    
            
            //content.CreateElement(0, "div", icon =>
            //{
            //    icon.CreateComponent<TIcon>(0, attributes: new
            //    {
            //        Name = Icon,
            //        AdditionalClass = $"t-popconfirm__icon--{Theme.Value}"
            //    });

            //    icon.Div().Class("t-popconfirm__inner").Content(ConfirmContent).Close();

            //}, new { @class = "t-popconfirm__body" });

            //content.CreateElement(1, "div", buttons =>
            //{
            //    BuildButton(buttons, 0, CancelBtnText, OnCancel, CancelBtnVarient, CancelBtnTheme, "t-popconfirm__cancel");
            //    BuildButton(buttons, 0, ConfirmBtnText, OnConfirm, ConfirmBtnVarient, ConfirmBtnTheme, "t-popconfirm__confirm");

            //}, new { @class = "t-popconfirm__buttons" });
        //})
    

    /// <summary>
    /// 构建按钮。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    /// <param name="text">按钮文本。</param>
    /// <param name="callback">按钮事件。</param>
    /// <param name="varient">按钮变体样式。</param>
    /// <param name="theme">按钮的主题。</param>
    /// <param name="css">其他 css。</param>
    void BuildButton(RenderTreeBuilder builder,int sequence,string? text,EventCallback<PopConfirmEventArgs> callback,ButtonVarient varient,Theme? theme,string? css)
    {
        builder.CreateComponent<TButton>(sequence, text, new
        {
            Theme = theme,
            Varient = varient,
            OnClick = HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, async (_) =>
            {
                PopConfirmEventArgs e = new();
                await callback.InvokeAsync(e);

                if ( e.Closed )
                {
                    await Hide();
                }
            }),
            Size=BtnSize,
            AdditionalClass = css
        });
    }
}

/// <summary>
/// 表示 <see cref="TPopConfirm"/> 组件的事件参数。
/// </summary>
public class PopConfirmEventArgs : EventArgs
{
    /// <summary>
    /// 获取或设置一个布尔值，表示是否关闭弹出层。默认是 <c>true</c>。
    /// </summary>
    public bool Closed { get; set; } = true;
}
