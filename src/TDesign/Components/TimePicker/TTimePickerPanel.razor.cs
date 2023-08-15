using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 时间选择器面板。
/// </summary>
partial class TTimePickerPanel
{
    [Parameter]public TimeSpan Value { get; set; }
    [Parameter]public EventCallback<TimeSpan> ValueChanged { get; set; }

    [Parameter]public bool ShowFooter { get; set; }

    TimeSpan CurrentValue => new(CurrentHours, CurrentMinutes, CurrentSeconds);

    int CurrentHours { get; set; }
    int CurrentMinutes { get; set; }
    int CurrentSeconds { get; set; }

    Dictionary<int, ElementReference?> _hourStores = new();
    Dictionary<int,ElementReference?> _minuteStores = new();
    Dictionary<int, ElementReference?> _secondStores = new();

    IJSModule _module;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        CurrentHours = Value.Hours;
        CurrentMinutes = Value.Minutes;
        CurrentSeconds = Value.Seconds;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            _module = await JS.ImportTDesignModuleAsync("timepicker");
        }
    }

    ValueTask ClickToScroll(ElementReference? element,Action action)
    {
        action();
        ChangeTime();
        return _module.Module.InvokeVoidAsync("timepicker.clickToScroll", element);
    }

    ValueTask WheelToScroll(WheelEventArgs e,ElementReference? element)
    {
        return _module.Module.InvokeVoidAsync("timepicker.wheelToScroll", e,element);
    }

    Task ChangeTime()
    {
        Value = CurrentValue;
        return ValueChanged.InvokeAsync(Value);
    }

    /// <summary>
    /// 此刻
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    async Task Now(MouseEventArgs e)
    {
        var now = TimeSpan.FromTicks(DateTime.Now.Ticks);
        CurrentHours = now.Hours;
        CurrentMinutes = now.Minutes;
        CurrentSeconds = now.Seconds;

        await ChangeTime();
    }
}
