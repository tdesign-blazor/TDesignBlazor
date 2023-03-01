using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace TDesign;

/// <summary>
/// 支持 <see cref="TInputRadio{TValue}"/> 控件的容器。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
[ParentComponent(IsFixed = true)]
[CascadingTypeParameter(nameof(TValue))]
public class TInputRadioContainer<TValue> : BlazorComponentBase, IHasChildContent,IHasInputValue<TValue>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///  执行当 <see cref="TInputRadio{TValue}"/> 触发的事件。
    /// </summary>
    [Parameter] public EventCallback<string> OnValueSelected { get; set; }

    /// <summary>
    /// 内部事件，当 <see cref="TInputRadio{TValue}"/> 组件被点击发生改变时触发。
    /// </summary>
    internal EventCallback<ChangeEventArgs> ChangeEventCallback { get; set; }

    /// <summary>
    /// Gets the selected value.
    /// </summary>
    internal TValue? SelectedValue => this.Value;

    /// <inheritdoc/>
    [CascadingParameter]public EditContext? CascadedEditContext { get; set; }

    /// <inheritdoc/>
    [Parameter]public TValue? Value { get; set; }
    /// <inheritdoc/>
    [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }
    /// <inheritdoc/>
    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }

    /// <summary>
    /// 表示当组内的单选框发生变化时的通知。
    /// </summary>
    internal event Action NotifyRadioInputRendered;
    string? _oldValue = default;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        var newValue = this.GetValueAsString();
        ChangeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value =>
        {
            this.GetCurrentValueAsString(__value);
            _ = OnValueSelected.InvokeAsync(__value);
        }
        , newValue);

        if ( _oldValue != newValue )
        {
            _oldValue = newValue;
            NotifyRadioInputRendered?.Invoke();
        }
    }
}
