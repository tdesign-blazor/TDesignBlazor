using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 固钉组件。
/// </summary>
public class TAffix : BlazorComponentBase, IHasChildContent
{
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

    #region Private Members

    private readonly string affixId = $"affix-{Guid.NewGuid()}";

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
    private int _affixFixedTopValue = 0;

    /// <summary>
    /// 组件底部固定时的高度值，top值。
    /// </summary>
    private int _affixFixedBottomValue = 0;

    /// <summary>
    /// top 固定。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="top"></param>
    /// <param name="offset"></param>
    private void TryFix(bool value, int top, int offset)
    {
        _fixed = value;
        if (_fixed)
        {
            _fixedStyle = $"top: {offset + top}px;";
        }
        else
        {
            _fixedStyle = string.Empty;
        }
        StateHasChanged();
    }

    #endregion END Private Members

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
            await popperWrapper.InvokeVoidAsync("affix.init", affixId, Container, objRef);
            var y = await popperWrapper.InvokeAsync<int>("affix.positionY", affixId);
            _affixYInit = y;
            await DebugMsg($"[debug]onAfaterRenderAsync affix.positon.y:{y}");
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
            attributes.Add("id", affixId);
        }
        base.BuildAttributes(attributes);
    }

    /// <summary>
    /// js 调用的方法，onscroll事件中调用并回传当前的top和bottom值。
    /// </summary>
    /// <param name="containerScrollTop"></param>
    /// <param name="containerY"></param>
    /// <param name="containerHeight"></param>
    /// <param name="affixY">组件当前位置，当前到窗口顶端的高度</param>
    /// <param name="affixHeight"></param>
    [JSInvokable]
    public void OnScrollChanged(int containerScrollTop, int containerY, int containerHeight, int containerScrollHeight, int affixY, int affixHeight)
    {
        //if (!_fixed)
        //{
        //    _affixYInit = affixY;
        //}
        //距离当前容器顶部的高度，组件原始位置到窗口顶部的高度 - 容器到窗口顶部的高度 - 容器滚动条卷去的高度
        var top = _affixYInit - containerY - containerScrollTop;
        if (top < 0)
        {
            top = 0;
        }
        if (OffsetTop > 0)
        {
            var isFixedTop = top <= OffsetTop;
            if (isFixedTop && _affixFixedTopValue == 0)
            {
                _affixFixedTopValue = containerY;
            }
            if (!isFixedTop && _affixFixedTopValue != 0)
            {
                _affixFixedTopValue = 0;
            }
            TryFix(isFixedTop, _affixFixedTopValue, OffsetTop);
            //await DebugMsg($"[debug]onscroll _affixOffsetTop:{_affixYInit}, top:{top}, containerScrollTop:{containerScrollTop}, container.y:{containerY}, fixedTop:{_affixFixedBottomValue}, isFixedTop:{isFixedTop}");
        }
        //await DebugMsg($"[debug]onscroll top:{top}, containerScrollTop:{containerScrollTop}, y:{containerY}, fixedTop:{_affixFixedBottomValue}");
        //// 如果固定了 OffsetTop 不再执行判断是否固定 OffsetBottom 的逻辑
        //if (OffsetBottom == 0)
        //{
        //    return;
        //}
        ////var bottom = containerHeight - top;

        //var currentTop = containerHeight - OffsetBottom;

        ////if (bottom < 0)
        ////{
        ////    bottom = 0;
        ////}
        //var isFixedBottom = top >= currentTop;
        //if (isFixedBottom && _affixFixedBottomValue == 0)
        //{
        //    _affixFixedBottomValue = containerOffsetTop;
        //}
        //if (!isFixedBottom && _affixFixedBottomValue != 0)
        //{
        //    _affixFixedBottomValue = 0;
        //}
        //await DebugMsg($"[debug]onscroll top:{top}, containerScrollTop:{containerScrollTop}, y:{containerOffsetTop}, fixedTop:{_affixFixedBottomValue}, fixed:{isFixedBottom}");
        //TryFix(isFixedBottom, _affixFixedBottomValue, currentTop);
    }

    private async Task DebugMsg(string msg)
    {
        var popperWrapper = await JS.Value.InvokeAsync<IJSObjectReference>("import", "./_content/TDesign/tdesign-blazor.js");
        await popperWrapper.InvokeVoidAsync("affix.show", msg);
    }
}

