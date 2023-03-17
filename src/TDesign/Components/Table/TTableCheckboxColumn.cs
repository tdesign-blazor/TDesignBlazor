namespace TDesign;
/// <summary>
/// 表示呈现复选选控件的列。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
/// <typeparam name="TField">绑定的字段类型。</typeparam>
[CssClass("t-table__cell-check")]
public class TTableCheckboxColumn<TItem,TField> : TTableRadioColumn<TItem,TField>
{
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Table.IsSingleSelection = false;
    }

    /// <inheritdoc/>
    protected internal override RenderFragment? GetCellContent(int rowIndex,TItem item)
    {
        var value = CellTextFunc!(item);

        return builder => builder.Fluent().Element("label", "t-checkbox")
                                         .Class("t-is-checked", IsChecked(value))
                                         .Content(checkbox =>
                                         {
                                             checkbox.Fluent().Element("input", "t-checkbox__former")
                                                             .Attribute("value", value)
                                                             .Attribute("type", "checkbox")
                                                             .Callback<ChangeEventArgs>("onchange", this, e =>
                                                             {
                                                                 Table.SelectRow(rowIndex);
                                                             })
                                                         .Close();
                                             checkbox.Fluent().Span("t-checkbox__input").Close();
                                             checkbox.Fluent().Span("t-checkbox__label").Close();
                                         })
                                       .Close();
    }

    /// <inheritdoc/>
    protected override bool IsChecked(string? value)
    {
        var values = Table.SelectedRows.Select(m => CellTextFunc!(m.Item!));

        return values.Any(eachValue => eachValue is not null && eachValue.Equals(value));
    }
}
