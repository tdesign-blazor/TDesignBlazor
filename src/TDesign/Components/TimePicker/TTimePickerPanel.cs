using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 时间选择器面板。
/// </summary>
public partial class TTimePickerPanel:TDesignComponentBase,IHasValueBound<TimeSpan>
{
    [Parameter]public TimeSpan Value { get; set; }
    [Parameter]public EventCallback<TimeSpan> ValueChanged { get; set; }

    int CurrentHour { get; set; }
    int CurrentMinute { get; set; }
    int CurrentSecond { get; set; }

    ElementReference? _hourElement;
    ElementReference? _minuteElement;
    ElementReference? _secondElement;

    IJSModule _module;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            _module = await JS.ImportTDesignModuleAsync("timepicker");
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Div("t-time-picker__panel")
            .Content(pannel =>
            {
                pannel.Div("t-time-picker__panel-section-body")
                    .Content(body =>
                    {
                        body.Div("t-time-picker__panel-body")
                            .Content(mask =>
                            {
                                mask.Div("t-time-picker__panel-body-active-mask").Content(b => b.ForEach("div", 3)).Close();

                                BuildScroll(mask, 24, CurrentHour, value => CurrentHour = value, el=> _hourElement=el); //时
                                BuildScroll(mask, 60, CurrentMinute, value => CurrentMinute = value, el => _minuteElement = el);//分
                                BuildScroll(mask, 60, CurrentSecond, value => CurrentSecond = value, el => _secondElement = el);//秒
                            })
                            .Close();
                    })
                    .Close();
            })
            .Close();
    }


    void BuildScroll(RenderTreeBuilder builder, int times, int current, Action<int> clickHandler, Action<ElementReference?> captureElementHandler)
    {
        builder.Element("ul", "t-time-picker__panel-body-scroll")
            .Content(li =>
            {
                li.ForEach("li", times, b =>
                {
                    var value = b.index;

                    b.attribute.Class("t-time-picker__panel-body-scroll-item")
                    .Class("t-is-current", value == current)
                    .Data("value",value)
                    .Callback<WheelEventArgs>("onwheel", this, e =>
                    {
                        _module.Module.InvokeVoidAsync("timepicker.scroll", _hourElement, e.MovementY);
                    })
                    .Callback<MouseEventArgs>("onclick", this, e =>
                    {
                        clickHandler(value);
                        ChangeTime();
                    })
                    .Ref(captureElementHandler)
                    .Content(value.ToString().PadLeft(2, '0'))
                    ;
                });
            })
            .Close();
    }

    Task ChangeTime()
    {
        Value = new(CurrentHour, CurrentMinute, CurrentSecond);
        return ValueChanged.InvokeAsync(Value);
    }
}
