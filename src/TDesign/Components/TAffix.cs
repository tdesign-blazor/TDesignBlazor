using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 固钉。在指定的范围内，将元素固定不动。
/// </summary>
public class TAffix : TDesignAdditionParameterWithChildContentComponentBase
{
    #region Parameters
    /// <summary>
    /// 设置指定滚动的容器的元素 id。<c>null</c> 时，则使用 body 元素。
    /// </summary>
    [Parameter]public string? ContainerId { get; set; }
    /// <summary>
    /// 距离容器顶部达到指定距离后触发固定，默认值0 。
    /// </summary>
    [Parameter] public int OffsetBottom { get; set; }

    /// <summary>
    /// 距离容器底部达到指定距离后触发固定，默认值0 。
    /// </summary>
    [Parameter] public int OffsetTop { get; set; }

    /// <summary>
    ///  设置一个回调方法，当固定状态发生变化时触发。
    /// </summary>
    [Parameter] public EventCallback<AffixedChangeEventArgs> OnFixedChange { get; set; }

    #endregion END Parameters

    #region Private Members

    private bool _fixed = false;

    private string _fixedStyle = string.Empty;

    private const string CSS_NAME = "t-affix";

    /// <summary>
    /// 组件原始位置：组件初始化时，距离窗口顶端的高度值。
    /// </summary>
    private decimal _affixYInit = 0;

    /// <summary>
    /// 组件顶端固定时的高度值，top值。
    /// </summary>
    private decimal _FixedTopValue = 0;

    /// <summary>
    /// top 固定。
    /// </summary>
    /// <param name="isFixed"></param>
    /// <param name="top"></param>
    private async Task ChangeFixState(bool isFixed, decimal top)
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
        //StateHasChanged();
        if (changed)
        {
            await OnFixedChange.InvokeAsync(new(_fixed, top));
        }
    }

    private bool TryFixedOffsetTop(decimal containerY, decimal containerScrollTop, out decimal value)
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

    private bool TryFixedOffsetBottom(decimal containerY, decimal containerHeight, decimal containerScrollTop, out decimal value)
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
            var module = await JS.ImportTDesignModuleAsync("affix");
            await module.Module.InvokeVoidAsync("affix.init", ContainerId, JSInvokeMethodFactory.Create<decimal, decimal, decimal, Task>(OnScrollChanged));
            _affixYInit = await module.Module.InvokeAsync<decimal>("affix.positionY", ContainerId);
        }
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
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append(_fixedStyle, _fixed);
    }

    #endregion END Override Methods

    /// <summary>
    /// js 调用的方法，onscroll事件中调用并回传当前的top和bottom值。
    /// </summary>
    /// <param name="containerScrollTop"></param>
    /// <param name="containerY">容器到窗口顶端的坐标值</param>
    /// <param name="containerHeight">容器的可见高度</param>
    async Task OnScrollChanged(decimal containerScrollTop, decimal containerY, decimal containerHeight)
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
    /// <param name="isFixed">是否已经被固定住。</param>
    /// <param name="top">固定时距离顶部的距离。</param>
    internal AffixedChangeEventArgs(bool isFixed, decimal top)
    {
        Fixed = isFixed;
        FixedTop = top;
    }
    /// <summary>
    /// 获取一个布尔值，表示容器已经被固定。
    /// </summary>
    public bool Fixed { get; }

    /// <summary>
    /// 获取固定时距离窗口顶端的距离。
    /// </summary>
    public decimal FixedTop { get; }
}

