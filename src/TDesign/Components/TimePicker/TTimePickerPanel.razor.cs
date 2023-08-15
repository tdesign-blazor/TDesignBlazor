using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 时间选择器面板。
/// </summary>
partial class TTimePickerPanel
{
    [Parameter]public TimeSpan Value { get; set; }
    [Parameter]public EventCallback<TimeSpan> ValueChanged { get; set; }

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

    ValueTask ScrollTo(ElementReference? element,Action action)
    {
        action();
        ChangeTime();
        return _module.Module.InvokeVoidAsync("timepicker.clickToScroll", element);
    }

    Task ChangeTime()
    {
        Value = CurrentValue;
        return ValueChanged.InvokeAsync(Value);
    }
}
