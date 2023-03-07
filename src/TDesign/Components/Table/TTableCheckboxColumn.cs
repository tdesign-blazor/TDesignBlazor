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
        CascadingGenericTable.IsSingleSelection = false;
    }

    internal override RenderFragment? GetColumnContent(params object[]? args)
    {
        var rowData = GetRowData(args);
        var value = GetValue(rowData);

       return builder => builder.Fluent().Element("label", "t-checkbox")
                                        .Class("t-is-checked", IsChecked(value))
                                        .Content(checkbox =>
                                        {
                                            checkbox.Fluent().Element("input", "t-checkbox__former")
                                                            .Attribute("value", value, value is not null)
                                                            .Attribute("type", "checkbox")
                                                            .Callback<ChangeEventArgs>("onchange",this,e=>
                                                            {
                                                                var getRowIndex = FindRowIndex(value);
                                                                CascadingGenericTable.SelectRow(getRowIndex);
                                                            })
                                                        .Close();
                                            checkbox.Fluent().Span("t-checkbox__input").Close();
                                            checkbox.Fluent().Span("t-checkbox__label").Close();
                                        })
                                      .Close();
    }

    /// <inheritdoc/>
    protected override bool IsChecked(object? value)
    {
        var values = CascadingGenericTable.SelectedRows.Select(m => m.Item?.GetType()?.GetProperty(Field!)?.GetValue(m.Item));

        return values.Any(eachValue => eachValue is not null && eachValue.Equals(value));
    }
}
