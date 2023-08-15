using System.Linq.Expressions;

namespace TDesign;

partial class TTimePicker
{
    [Parameter]public TimeSpan Value { get; set; }
    [Parameter]public TimeSpan ValueChanged { get; set; }
    [Parameter]public Expression<Func<TimeSpan>> ValueExpression { get; set; }

    [Parameter] public string? Format { get; set; } = "HH:mm:ss";

    string InputText { get; set; }
}
