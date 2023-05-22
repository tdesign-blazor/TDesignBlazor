using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 表示作为折叠面板的内容。必须放在 <see cref="TCollapse"/> 组件中。
/// </summary>
[CssClass("t-collapse-panel")]
[ChildComponent(typeof(TCollapse))]
public class TCollapsePanel : TDesignComponentBase, IHasChildContent, IHasActive, IHasDisabled
{
    /// <summary>
    /// 初始化 <see cref="TCollapsePanel"/> 类的新实例。
    /// </summary>
    public TCollapsePanel()
    {
        TitleContent ??= new(builder => builder.AddContent(0, Title));
    }
    /// <summary>
    /// 级联 <see cref="TCollapse"/> 组件。
    /// </summary>
    [CascadingParameter] public TCollapse CascadingCollaspe { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置标题部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }
    /// <summary>
    /// 设置标题字符串。若设置了 <see cref="TitleContent"/> 属性的值，则该属性无效。
    /// </summary>
    [Parameter] public string? Title { get; set; }
    /// <summary>
    /// 右侧操作的内容。
    /// </summary>
    [Parameter] public RenderFragment? OperationContent { get; set; }
    /// <summary>
    /// <c>true</c> 表示面板是展开状态，否则为折叠状态。
    /// </summary>
    [Parameter] public bool Active { get; set; }
    /// <summary>
    /// <c>true</c> 表示禁用面板，否则为 <c>false</c>。
    /// </summary>
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            #region Header
            builder.CreateElement(0, "div", title =>
            {
                if (CascadingCollaspe.RightTIcon.HasValue && CascadingCollaspe.RightTIcon == false)
                {
                    BuildTIcon(title, 0);
                }

                builder.AddContent(1, TitleContent);

                builder.CreateElement(2, "div", attributes: new { @class = "t-collapse-panel__header--blank" });

                if (CascadingCollaspe.RightTIcon.HasValue && CascadingCollaspe.RightTIcon.Value)
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
            Name = !Active ? (CascadingCollaspe.RightTIcon.HasValue && CascadingCollaspe.RightTIcon.Value ? IconName.ChevronLeft : IconName.ChevronRight) : IconName.ChevronDown,
            AdditionalClass = HtmlHelper.Instance.Class()
                                    .Append("t-collapse-panel__icon")
                                    .Append("t-collapse-panel__icon--left", CascadingCollaspe.RightTIcon.HasValue && !CascadingCollaspe.RightTIcon.Value)
                                    .Append("t-collapse-panel__icon--right", CascadingCollaspe.RightTIcon.HasValue && CascadingCollaspe.RightTIcon.Value).ToString(),
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
            .Append("display:none", !Active)
            .Append("")
        });
    }

    /// <summary>
    /// 执行展开或折叠的操作。
    /// </summary>
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
                    panel.Active = false;
                    await panel.Refresh();
                }
            }
        }
        Active = !Active;
        await this.Refresh();
    }
}
