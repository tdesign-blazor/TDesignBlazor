using Microsoft.AspNetCore.Components.Rendering;
using TDesign.Specifications;

namespace TDesign;
/// <summary>
/// 表示作为折叠面板的内容。必须放在 <see cref="TCollapse"/> 组件中。
/// </summary>
[CssClass("t-collapse-panel")]
[ChildComponent(typeof(TCollapse))]
public class TCollapsePanel : TDesignAdditionParameterWithChildContentComponentBase, IHasDisabled,IHasTitleFragment,IHasTitleText
{
    /// <summary>
    /// 级联 <see cref="TCollapse"/> 组件。
    /// </summary>
    [CascadingParameter] public TCollapse CascadingCollaspe { get; set; }
    /// <summary>
    /// 设置标题部分的内容。
    /// </summary>
    [ParameterApiDoc("标题部分的内容")]
    [Parameter] public RenderFragment? TitleContent { get; set; }
    /// <summary>
    /// 设置标题字符串。若设置了 <see cref="TitleContent"/> 属性的值，则该属性无效。
    /// </summary>
    [ParameterApiDoc("标题字符串，若设置 TitleContent，则该值无效")]
    [Parameter] public string? TitleText { get; set; }
    /// <summary>
    /// 右侧操作的内容。
    /// </summary>
    [ParameterApiDoc("右侧操作的内容")]
    [Parameter] public RenderFragment? OperationContent { get; set; }
    /// <summary>
    /// <c>true</c> 表示面板是展开状态，否则为折叠状态。
    /// </summary>
    [ParameterApiDoc("是否为展开状态")]
    [Parameter] public bool Expaned { get; set; }
    /// <summary>
    /// <c>true</c> 表示禁用面板，否则为 <c>false</c>。
    /// </summary>
    [ParameterApiDoc("禁用状态")]
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            #region Header
            builder.CreateElement(0, "div", title =>
            {
                if (CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon == false)
                {
                    BuildTIcon(title, 0);
                }

                builder.AddContent(1, TitleContent);

                builder.CreateElement(2, "div", attributes: new { @class = "t-collapse-panel__header--blank" });

                if (CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon.Value)
                {
                    BuildTIcon(title, 3);
                }

                if (OperationContent is not null)
                {
                    builder.AddContent(5, OperationContent);
                }
            }, new
            {
                @class = HtmlHelper.Instance.Class().Append("t-collapse-panel__header").Append("t-is-clickable", !CascadingCollaspe.IconToggle && !Disabled),
                onclick = HtmlHelper.Instance.Callback().Create(this, () =>
                {
                    if (!CascadingCollaspe.IconToggle)
                    {
                        Toggle();
                    }
                })
            });
            #endregion

            BuildContent(content, 0);

        }, new { @class = "t-collapse-panel__wrapper" });
    }



    private void BuildTIcon(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<TIcon>(sequence, attributes: new
        {
            Name = !Expaned ? (CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon.Value ? IconName.ChevronLeft : IconName.ChevronRight) : IconName.ChevronDown,
            AdditionalClass = HtmlHelper.Instance.Class()
                                    .Append("t-collapse-panel__icon")
                                    .Append("t-collapse-panel__icon--left", CascadingCollaspe.RightIcon.HasValue && !CascadingCollaspe.RightIcon.Value)
                                    .Append("t-collapse-panel__icon--right", CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon.Value).ToString(),
            onclick = HtmlHelper.Instance.Callback().Create(this, () =>
                {
                    if (CascadingCollaspe.IconToggle)
                    {
                        Toggle();
                    }

                })
        });
    }

    void BuildContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content => content.CreateElement(0, "div", ChildContent, new
        {
            @class = "t-collapse-panel__content"
        }), new
        {
            @class = HtmlHelper.Instance.Class()
            .Append("t-collapse-panel__body"),
            style = HtmlHelper.Instance.Style()
            .Append("display:none", !Expaned)
            .Append("")
        });
    }

    /// <summary>
    /// 执行展开或折叠的操作。
    /// </summary>
    [MethodApiDoc("执行展开或折叠的操作")]
    public async Task Toggle()
    {
        if (Disabled)
        {
            return;
        }

        if (CascadingCollaspe.Mutex)
        {
            for (int i = 0; i < CascadingCollaspe.ChildComponents.Count; i++)
            {
                var child = CascadingCollaspe.ChildComponents[i];
                if (child is TCollapsePanel panel)
                {
                    panel.Expaned = false;
                    await panel.Refresh();
                }
            }
        }
        Expaned = !Expaned;
        await this.Refresh();
    }
}
