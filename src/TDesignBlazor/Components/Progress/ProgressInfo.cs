using Microsoft.AspNetCore.Components.Rendering;

using OneOf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor
{
    [HtmlTag("div")]
    [CssClass("t-progress__info")]
    internal class ProgressInfo : BlazorComponentBase, IHasChildContent
    {
        /// <summary>
        /// 进度条风格
        /// </summary>
        [Parameter] public ProgressThemeType? Theme { get; set; } = ProgressThemeType.Line;
        [Parameter] public Status? Status { get; set; }
         public RenderFragment? ChildContent { get; set; }
        [Parameter] public int? Percentage { get; set; } = 0;
        /// <summary>
        /// 进度百分比
        /// </summary>
        [Parameter] public OneOf<string, bool> Label { get; set; } = true;
        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            var icon = GetIcon();
            var isLable = GetIsLabel();
            var lableText = GetLable();
            if ((Status == TDesignBlazor.Status.None || Status == TDesignBlazor.Status.Default) && isLable)
            {
                builder.AddContent(sequence + 1, lableText);
            }
            if ((Status != TDesignBlazor.Status.None && Status != TDesignBlazor.Status.Default) && isLable && Theme != ProgressThemeType.Circle)
            {
                builder.CreateComponent<Icon>(sequence + 1, attributes: new { @class = $"t-icon t-icon-{icon}-circle-filled t-progress__icon" });
            }
            if ((Status != TDesignBlazor.Status.None && Status != TDesignBlazor.Status.Default) && isLable && Theme == ProgressThemeType.Circle)
            {
                if (!lableText.EndsWith("%"))
                {
                    builder.AddContent(sequence + 1, lableText);
                }
                else
                {
                    builder.CreateComponent<Icon>(sequence + 1, attributes: new { @class = $"t-icon t-icon-{icon} t-progress__icon" });
                }


            }
        }
        /// <summary>
        /// 获取状态图标
        /// </summary>
        /// <returns></returns>
        private string GetIcon()
        {
            switch (Status)
            {
                case TDesignBlazor.Status.Default:
                case TDesignBlazor.Status.None:
                    return "";
                case TDesignBlazor.Status.Warning:
                    return "error";

                case TDesignBlazor.Status.Error:
                    return "close";

                case TDesignBlazor.Status.Success:
                    return "check";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 获取是否显示Lable
        /// </summary>
        /// <returns></returns>
        private bool GetIsLabel()
        {
            return Label.Match<bool>(
                a => a != "",
                b => b);
        }

        /// <summary>
        /// 获取Lable文本
        /// </summary>
        /// <returns></returns>
        private string GetLable()
        {
            return Label.Match<string>(
                a => a,
                b => Percentage.ToString() + "%");
        }
    }
}
