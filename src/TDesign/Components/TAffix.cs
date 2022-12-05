using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 固钉组件。
/// </summary>
public class TAffix : BlazorComponentBase, IHasChildContent
{
    #region Parameters

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 距离容器顶部达到指定距离后触发固定，默认值0 。
    /// </summary>
    [Parameter] public int OffsetBottom { get; set; }

    /// <summary>
    /// 距离容器底部达到指定距离后触发固定，默认值0 。
    /// </summary>
    [Parameter] public int OffsetTop { get; set; }

    /// <summary>
    /// 固钉定位层级，样式默认为 500 。
    /// </summary>
    [Parameter] public int ZIndex { get; set; } = ZINDEX_DEFAULT;

    /// <summary>
    /// 指定滚动的容器。滚动容器不是body时，传入滚动容器的id。
    /// </summary>
    [Parameter] public string? Container { get; set; }

    /// <summary>
    ///  执行当 <see cref="TAffix"/> 的固定状态发生变化时的事件。
    /// </summary>
    [Parameter] public EventCallback<AffixedChangeEventArgs> FixedChange { get; set; }

    #endregion END Parameters

    #region Private Members

    private readonly string _affixId = $"affix-{Guid.NewGuid()}";

    /// <summary>
    /// z-index 默认值。
    /// </summary>
    private const int ZINDEX_DEFAULT = 500;

    private bool _fixed = false;

    private string _fixedStyle = string.Empty;

    private const string CSS_NAME = "t-affix";

    /// <summary>
    /// 组件原始位置：组件初始化时，距离窗口顶端的高度值。
    /// </summary>
    private int _affixYInit = 0;

    /// <summary>
    /// 组件顶端固定时的高度值，top值。
    /// </summary>
    private int _FixedTopValue = 0;

    /// <summary>
    /// top 固定。
    /// </summary>
    /// <param name="isFixed"></param>
    /// <param name="top"></param>
    private async Task ChangeFixState(bool isFixed, int top)
    {
        var changed = isFixed != _fixed;
        _fixed = isFixed;
        if (_fixed)
        {
            _fixedStyle = $"top: {top}px;";
        }
        else
        {
            _fixedStyle = string.Empty;
        }
        StateHasChanged();
        if (changed)
        {
            await FixedChange.InvokeAsync(new AffixedChangeEventArgs(_fixed, top));
        }
    }

    private bool TryFixedOffsetTop(int containerY, int containerScrollTop, out int value)
    {
        var top = _affixYInit - containerY - containerScrollTop;
        value = 0;
        if (top < 0)
        {
            top = 0;
        }
        var isFixed = false;
        if (OffsetTop > 0)
        {
            isFixed = top <= OffsetTop;
            if (isFixed)
            {
                value = containerY + OffsetTop;
            }
        }
        return isFixed;
    }

    private bool TryFixedOffsetBottom(int containerY, int containerHeight, int containerScrollTop, out int value)
    {
        var top = _affixYInit - containerY - containerScrollTop;
        value = 0;
        if (top < 0)
        {
            top = 0;
        }
        var bottom = containerHeight - top;
        var isFixed = false;
        if (OffsetBottom > 0)
        {
            isFixed = bottom <= OffsetBottom;
            if (isFixed)
            {
                value = containerY + (containerHeight - OffsetBottom);
            }
        }
        return isFixed;
    }

    #endregion END Private Members

    #region Override Methods

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var objRef = DotNetObjectReference.Create(this);
            var popperWrapper = await JS.Value.InvokeAsync<IJSObjectReference>("import", "./_content/TDesign/tdesign-blazor.js");
            await popperWrapper.InvokeVoidAsync("affix.init", Container, objRef);
            var y = await popperWrapper.InvokeAsync<int>("affix.positionY", _affixId);
            _affixYInit = y;
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (!_fixed && builder.Contains(CSS_NAME))
        {
            builder.Remove(CSS_NAME);
        }
        else
        {
            builder.Append(CSS_NAME, _fixed);
        }
        base.BuildCssClass(builder);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append(_fixedStyle, _fixed);
        builder.Append($"z-index: {ZIndex}", _fixed && ZIndex != ZINDEX_DEFAULT);
        base.BuildStyle(builder);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="attributes"></param>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        if (!attributes.ContainsKey("id"))
        {
            attributes.Add("id", _affixId);
        }
        base.BuildAttributes(attributes);
    }

    #endregion END Override Methods

    /// <summary>
    /// js 调用的方法，onscroll事件中调用并回传当前的top和bottom值。
    /// </summary>
    /// <param name="containerScrollTop"></param>
    /// <param name="containerY">容器到窗口顶端的坐标值</param>
    /// <param name="containerHeight">容器的可见高度</param>
    [JSInvokable]
    public async Task OnScrollChanged(int containerScrollTop, int containerY, int containerHeight)
    {
        //距离当前容器顶部的高度，组件原始位置到窗口顶部的高度 - 容器到窗口顶部的高度 - 容器滚动条卷去的高度
        var topFixed = TryFixedOffsetTop(containerY, containerScrollTop, out var value);
        if (topFixed && _FixedTopValue == 0)
        {
            _FixedTopValue = value;
        }

        var bottomFixed = TryFixedOffsetBottom(containerY, containerHeight, containerScrollTop, out var bottomValue);
        if (bottomFixed && _FixedTopValue == 0)
        {
            _FixedTopValue = bottomValue;
        }
        if (!topFixed && !bottomFixed)
        {
            _FixedTopValue = 0;
        }
        await ChangeFixState(topFixed || bottomFixed, _FixedTopValue);
    }
}

/// <summary>
/// 提供<see cref="TAffix.FixedChange"/>事件的回调参数模型。
/// </summary>
public class AffixedChangeEventArgs
{
    /// <summary>
    /// 构建<see cref="AffixedChangeEventArgs"/>实例。
    /// </summary>
    /// <param name="affixed"></param>
    /// <param name="top"></param>
    public AffixedChangeEventArgs(bool affixed, int top)
    {
        Affixed = affixed;
        Top = top;
    }
    /// <summary>
    /// 是否固定。
    /// </summary>
    public bool Affixed { get; set; }

    /// <summary>
    /// 固定时距离窗口顶端的距离。
    /// </summary>
    public int Top { get; set; }
}

