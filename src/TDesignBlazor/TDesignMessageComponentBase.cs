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
    /// 显示的标题。
    /// </summary>
    [Parameter] public string? Title { get; set; }
    /// <summary>
    /// 具备标题的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }
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
