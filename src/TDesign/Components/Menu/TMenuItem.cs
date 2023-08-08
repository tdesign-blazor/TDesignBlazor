using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;

namespace TDesign;

/// <summary>
/// 导航菜单的项。
/// </summary>
[CssClass("t-menu__item")]
[ChildComponent(typeof(TSubMenu), Optional = true)]
[ChildComponent(typeof(TMenu))]
public class TMenuItem : TDesignAdditionParameterWithChildContentComponentBase, IHasDisabled, IHasActive
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [CascadingParameter] public TMenu CascadingMenu { get; set; }
    [CascadingParameter] public TSubMenu? CascadingSubMenu { get; set; }

    internal bool CanNavigationChanged { get; set; } = true;

    /// <summary>
    /// 菜单高亮的匹配方式。
    /// </summary>
    [ParameterApiDoc("菜单高亮的匹配方式")]
    [Parameter] public NavLinkMatch Match { get; set; } = NavLinkMatch.All;
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [ParameterApiDoc("禁用状态")]
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }
    /// <summary>
    /// 导航的超链接。
    /// </summary>
    [ParameterApiDoc("导航的超链接")]
    [Parameter] public string? Link { get; set; }
    /// <summary>
    /// 前缀图标的名称。
    /// </summary>
    [ParameterApiDoc("前缀图标的名称")]
    [Parameter] public object? IconPrefix { get; set; }
    /// <summary>
    /// 后缀图标的名称。
    /// </summary>
    [ParameterApiDoc("后缀图标的名称")]
    [Parameter] public object? IconSuffix { get; set; }
    /// <summary>
    /// 选中状态。若为 <c>false</c> 则根据导航自动判断。
    /// </summary>
    [ParameterApiDoc("选中状态，若为 false 则根据导航自动判断。")]
    [Parameter] public bool Active { get; set; }

    public override string? GetTagName() => CascadingSubMenu is not null ? "div" : "li";

    public bool IsActive => _isActive;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        NavigationManager.LocationChanged += NavigationManager_LocationChanged;
    }

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        var shouldBeActiveNow = ShouldMatch(e.Location);
        if ( shouldBeActiveNow != _isActive )
        {
            _isActive = shouldBeActiveNow;
            StateHasChanged();
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (!string.IsNullOrEmpty(Link))
        {
            _hrefAbsolute = NavigationManager.ToAbsoluteUri(Link).AbsoluteUri;
        }
        _isActive = ShouldMatch(NavigationManager.Uri);
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (CascadingSubMenu is not null)
        {
            await CascadingSubMenu.Active();
        }
    }


    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Remove("t-is-active");

        builder.Append("t-is-active", Active || IsActive)
            .Append("t-menu__item--plain");
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<TIcon>(sequence, attributes: new { Name = IconPrefix }, condition: IconPrefix is not null);

        builder.CreateElement(sequence, "span", ChildContent, new { @class = "t-menu__content" });

        builder.CreateComponent<TIcon>(sequence, attributes: new { Name = IconSuffix }, condition: IconSuffix is not null);
    }

    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        if (CanNavigationChanged && !string.IsNullOrWhiteSpace(Link))
        {
            attributes["onclick"] = HtmlHelper.Instance.Callback().Create(this, NavigateTo);
        }
    }

    /// <inheritdoc/>
    protected override void DisposeComponentResources()
    {
        base.DisposeComponentResources();
        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
    }

    /// <summary>
    /// 跳转到指定的连接地址。
    /// </summary>
    void NavigateTo()
    {
        if ( !string.IsNullOrWhiteSpace(Link) )
        {
            NavigationManager.NavigateTo(Link);
        }
        CascadingSubMenu?.CollapseSubMenuItem();
    }

    private bool _isActive;
    private string? _hrefAbsolute;
    private bool ShouldMatch(string currentUriAbsolute)
    {
        if ( _hrefAbsolute == null )
        {
            return false;
        }

        if ( EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute) )
        {
            return true;
        }

        if ( Match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute) )
        {
            return true;
        }

        return false;
    }

    private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
    {
        Debug.Assert(_hrefAbsolute != null);

        if ( string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase) )
        {
            return true;
        }

        if ( currentUriAbsolute.Length == _hrefAbsolute.Length - 1 )
        {
            // Special case: highlight links to http://host/path/ even if you're
            // at http://host/path (with no trailing slash)
            //
            // This is because the router accepts an absolute URI value of "same
            // as base URI but without trailing slash" as equivalent to "base URI",
            // which in turn is because it's common for servers to return the same page
            // for http://host/vdir as they do for host://host/vdir/ as it's no
            // good to display a blank page in that case.
            if ( _hrefAbsolute[_hrefAbsolute.Length - 1] == '/'
                && _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase) )
            {
                return true;
            }
        }

        return false;
    }
    private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
    {
        var prefixLength = prefix.Length;
        if ( value.Length > prefixLength )
        {
            return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                && (
                    // Only match when there's a separator character either at the end of the
                    // prefix or right after it.
                    // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                    // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                    prefixLength == 0
                    || !char.IsLetterOrDigit(prefix[prefixLength - 1])
                    || !char.IsLetterOrDigit(value[prefixLength])
                );
        }
        else
        {
            return false;
        }
    }
}
