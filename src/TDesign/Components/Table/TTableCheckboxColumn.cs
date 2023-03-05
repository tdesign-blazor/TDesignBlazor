namespace TDesign;
/// <summary>
/// 表示呈现复选选控件的列。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
[CssClass("t-table__cell-check")]
public class TTableCheckboxColumn<TItem> : TTableRadioColumn<TItem>
{
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        CascadingTable.IsSingleSelection = false;
    }
    /// <inheritdoc/>
    protected override RenderFragment? GetColumnContent() => GetCheckboxFragment(IsChecked, Value);

    /// <summary>
    /// 获取复选框的内容。
    /// </summary>
    /// <param name="isChecked">是否选中、</param>
    /// <param name="value">复选框的值。</param>
    private RenderFragment? GetCheckboxFragment(bool isChecked, object? value = default)
        => builder => builder.Fluent().Element("label", "t-checkbox")
                                        .Class("t-is-checked", isChecked)
                                        .Content(checkbox =>
                                        {
                                            checkbox.Fluent().Element("input", "t-checkbox__former")
                                                            .Attribute("value", value, value is not null)
                                                            .Attribute("type", "checkbox")
                                                        .Close();
                                            checkbox.Fluent().Span("t-checkbox__input").Close();
                                            checkbox.Fluent().Span("t-checkbox__label").Close();
                                        })
                                      .Close();

    /// <inheritdoc/>
    protected override bool IsChecked
    {
        get
        {
            var values = CascadingTable.SelectedRows.Select(m => m.Item?.GetType()?.GetProperty(SelectionKey!)?.GetValue(m.Item));

            return values.Any(value => value is not null && value.Equals(Value));
        }
    }
}
