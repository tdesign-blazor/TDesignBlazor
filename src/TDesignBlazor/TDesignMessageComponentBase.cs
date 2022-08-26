namespace TDesignBlazor;
/// <summary>
/// 消息组件的基类。
/// </summary>
public abstract class TDesignMessageComponentBase : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 提醒主题。
    /// </summary>
    [Parameter] public Theme? Theme { get; set; }
    /// <summary>
    /// 图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; }

    protected string? GetThemeClass
    {
        get
        {
            if (Theme is null)
            {
                return string.Empty;
            }
            if (Theme == TDesignBlazor.Theme.Primary)
            {
                return "info";
            }
            if (Theme == TDesignBlazor.Theme.Danger)
            {
                return "error";
            }
            return Theme.Value;
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Icon ??= GetIconByTheme;
    }

    protected virtual IconName? GetIconByTheme
    {
        get
        {
            if (Theme is null)
            {
                return IconName.InfoCircle;
            }
            if (Theme == TDesignBlazor.Theme.Success)
            {
                return IconName.CheckCircleFilled;
            }
            if (Theme == TDesignBlazor.Theme.Danger)
            {
                return IconName.CloseCircleFilled;
            }
            return IconName.InfoCircleFilled;
        }
    }
}
