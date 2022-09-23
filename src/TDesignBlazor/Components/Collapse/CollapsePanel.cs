using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;
/// <summary>
/// 表示作为折叠面板的内容。必须放在 <see cref="Collapse"/> 组件中。
/// </summary>
[CssClass("t-collapse-panel")]
[ChildComponent(typeof(Collapse))]
public class CollapsePanel : BlazorComponentBase, IHasChildContent, IHasActive, IHasDisabled
{
    /// <summary>
    /// 初始化 <see cref="CollapsePanel"/> 类的新实例。
    /// </summary>
    public CollapsePanel()
    {
        TitleContent ??= new(builder => builder.AddContent(0, Title));
    }
    /// <summary>
    /// 级联 <see cref="Collapse"/> 组件。
    /// </summary>
    [CascadingParameter] public Collapse CascadingCollaspe { get; set; }
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
                if (CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon == false)
                {
                    BuildIcon(title, 0);
                }

                builder.AddContent(1, TitleContent);

                builder.CreateElement(2, "div", attributes: new { @class = "t-collapse-panel__header--blank" });

                if (CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon.Value)
                {
                    BuildIcon(title, 3);
                }

                if (OperationContent is not null)
                {
                    builder.AddContent(5, OperationContent);
                }
            }, new
            {
                @class = HtmlHelper.CreateCssBuilder().Append("t-collapse-panel__header").Append("t-is-clickable", !CascadingCollaspe.IconToggle && !Disabled),
                onclick = HtmlHelper.CreateCallback(this, Toggle, !CascadingCollaspe.IconToggle)
            });
            #endregion

            BuildContent(content, 0);

        }, new { @class = "t-collapse-panel__wrapper" });
    }



    private void BuildIcon(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<Icon>(sequence, attributes: new
        {
            Name = !Active ? (CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon.Value ? IconName.ChevronLeft : IconName.ChevronRight) : IconName.ChevronDown,
            AdditionalCssClass = HtmlHelper.CreateCssBuilder()
                                    .Append("t-collapse-panel__icon")
                                    .Append("t-collapse-panel__icon--left", CascadingCollaspe.RightIcon.HasValue && !CascadingCollaspe.RightIcon.Value)
                                    .Append("t-collapse-panel__icon--right", CascadingCollaspe.RightIcon.HasValue && CascadingCollaspe.RightIcon.Value).ToString(),
            onclick = HtmlHelper.CreateCallback(this, Toggle, CascadingCollaspe.IconToggle)
        });
    }

    void BuildContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content => content.CreateElement(0, "div", ChildContent, new
        {
            @class = "t-collapse-panel__content"
        }), new
        {
            @class = HtmlHelper.CreateCssBuilder()
            .Append("t-collapse-panel__body"),
            style = HtmlHelper.CreateStyleBuilder()
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
                if (child is CollapsePanel panel)
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
