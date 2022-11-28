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
    [Parameter] public int ZIndex { get; set; } = 500;

    /// <summary>
    /// 指定滚动的容器。滚动容器不是body时，传入滚动容器的id。
    /// </summary>
    [Parameter] public string? Container { get; set; }

    private bool _fixed = false;

    private string _fixedStyle = string.Empty;

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

        }
        await base.OnAfterRenderAsync(firstRender);
    }
    private const string CSS_NAME = "t-affix";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append(CSS_NAME, _fixed);
        base.BuildCssClass(builder);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append(_fixedStyle, _fixed);
        base.BuildStyle(builder);
    }

    /// <summary>
    /// js 调用的方法，onscroll事件中调用并回传当前的top和bottom值。
    /// </summary>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    /// <param name="height"></param>
    [JSInvokable]
    public void OnScrollChanged(int top, int bottom, int height)
    {
        if (FixedTop())
        {
            TryFixChange(top >= OffsetTop);
        }
        else
        {
            TryFixChange(bottom >= OffsetBottom);
        }
    }

    private bool FixedTop() => OffsetTop > 0;

    private void TryFixChange(bool value)
    {
        if (OffsetTop == 0 && OffsetBottom == 0)
        {
            return;
        }
        if (value != _fixed)
        {
            _fixed = value;
            if (_fixed)
            {
                _fixedStyle = FixedTop() ? $"top:{OffsetTop}px;" : $"bottom:{OffsetBottom}px;";
            }
            else
            {
                _fixedStyle = string.Empty;
            }
            StateHasChanged();
        }
    }
}

