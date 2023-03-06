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

    internal override RenderFragment? GetFieldValueContent(in int rowIndex,in int columnIndex,in TItem? rowData)
    {
        var value = GetValue(rowData);
        var index = rowIndex;

       return builder => builder.Fluent().Element("label", "t-checkbox")
                                        .Class("t-is-checked", IsChecked(value))
                                        .Attribute("rowIndex",index)
                                        .Content(checkbox =>
                                        {
                                            checkbox.Fluent().Element("input", "t-checkbox__former")
                                                            .Attribute("value", value, value is not null)
                                                            .Attribute("type", "checkbox")
                                                            .Callback<ChangeEventArgs>("onchange",this,e=>
                                                            {
                                                                var getRowIndex = CascadingGenericTable.TableData.FindIndex(m =>
                                                                {
                                                                    if(m.data is null )
                                                                    {
                                                                        return false;
                                                                    }
                                                                   return m.data.GetType().GetProperty(Field!).GetValue(m.data).Equals(value);
                                                                });
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
