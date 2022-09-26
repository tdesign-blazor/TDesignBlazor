using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor
{
    [HtmlTag("div")]
    [CssClass("t-progress__info")]
    internal class ProgressInfo : BlazorComponentBase, IHasChildContent
    {
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// 进度百分比
        /// </summary>
        [Parameter] public OneOf<string, bool> Label { get; set; } = true;

        [Parameter] public int? Percentage { get; set; } = 0;

        [Parameter] public Status? Status { get; set; }

        /// <summary>
        /// 进度条风格
        /// </summary>
        [Parameter] public ProgressThemeType? Theme { get; set; } = ProgressThemeType.Line;
        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            var icon = GetIcon();
            var isLable = GetIsLabel();
            var lableText = GetLable();
            if ((Status == TDesignBlazor.Status.None || Status == TDesignBlazor.Status.Default) && isLable)
            {
                builder.AddContent(sequence + 1, lableText);
            }
            if ((Status != TDesignBlazor.Status.None && Status != TDesignBlazor.Status.Default) && isLable)
            {
                if (Theme != ProgressThemeType.Circle)
                    builder.CreateComponent<Icon>(sequence + 1, attributes: new { @class = $"t-icon t-icon-{icon}-circle-filled t-progress__icon" });
                else
                {
                    if (!lableText.EndsWith("%"))
                        builder.AddContent(sequence + 1, lableText);
                    else
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
            return Status switch
            {
                TDesignBlazor.Status.Default or TDesignBlazor.Status.None => "",
                TDesignBlazor.Status.Warning => "error",
                TDesignBlazor.Status.Error => "close",
                TDesignBlazor.Status.Success => "check",
                _ => "",
            };
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
