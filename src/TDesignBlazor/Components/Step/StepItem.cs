using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 表示步骤的项。必须在 <see cref="StepGroup"/> 组件中使用。
/// </summary>
[CssClass("t-steps-item")]
[ChildComponent(typeof(StepGroup))]
public class StepItem : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// 级联参数。
    /// </summary>
    [CascadingParameter][NotNull] public StepGroup CascadingStepGroup { get; set; }
    /// <summary>
    /// 步骤的状态。
    /// </summary>
    [Parameter][CssClass("t-steps-item--")] public StepStatus Status { get; set; } = StepStatus.NotStart;
    /// <summary>
    /// 设置可点击的样式。
    /// </summary>
    [Parameter][CssClass("t-steps-item--clickable")] public bool Clickable { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置图标名称。默认自动计算当前步骤所在的数字。
    /// <para>
    /// 当 <see cref="StepGroup.Dot"/> 是 <c>true</c> 时无效。
    /// </para>
    /// </summary>
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 设置描述内容。
    /// </summary>
    [Parameter] public RenderFragment? DescriptionContent { get; set; }
    /// <summary>
    /// 设置附加内容。
    /// </summary>
    [Parameter] public RenderFragment? ExtraContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", inner =>
        {
            BuildIcon(inner, 0);
            BuildContent(inner, 1);

        }, new { @class = HtmlHelper.CreateCssBuilder().Append("t-steps-item__inner") });
    }

    private void BuildContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", ChildContent, new { @class = "t-steps-item__title" });
            content.CreateElement(1, "div", DescriptionContent, new { @class = "t-steps-item__description" }, DescriptionContent is not null);
            content.CreateElement(2, "div", ExtraContent, new { @class = "t-steps-item__extra" }, ExtraContent is not null);

        }, new { @class = "t-steps-item__content" });
    }

    /// <summary>
    /// 构建步骤的图标。
    /// </summary>
    /// <param name="builder"></param>
    private void BuildIcon(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", icon =>
        {
            if (CascadingStepGroup.Dot)
            {
                return;
            }

            if (Status == StepStatus.Error)
            {
                icon.CreateComponent<Icon>(0, attributes: new { Name = IconName.CloseCircle });
                return;
            }

            if (Icon is not null)
            {
                icon.CreateComponent<Icon>(0, attributes: new { Name = Icon });
                return;
            }

            icon.CreateElement(0, "div", num =>
            {
                switch (Status)
                {
                    case StepStatus.Finish:
                        num.CreateComponent<Icon>(0, attributes: new { Name = IconName.Check });
                        break;
                    default:
                        //数字
                        var number = CascadingStepGroup.ChildComponents.ToList().FindIndex(m => m == this);
                        num.AddContent(0, number + 1);
                        break;
                }

            }, new { @class = "t-steps-item__icon--number" });
        },
                    new
                    {
                        @class = HtmlHelper.CreateCssBuilder()
                        .Append("t-steps-item__icon")
                        .Append($"t-steps-item--{Status.GetCssClass()}")
                    });
    }
}

/// <summary>
/// 步骤的当前状态。
/// </summary>
public enum StepStatus
{
    /// <summary>
    /// 默认，未开始状态。
    /// </summary>
    [CssClass("default")] NotStart,
    /// <summary>
    /// 正在进行，会高亮步骤。
    /// </summary>
    [CssClass("process")] Process,
    /// <summary>
    /// 完成。
    /// </summary>
    Finish,
    /// <summary>
    /// 错误
    /// </summary>
    Error,
}