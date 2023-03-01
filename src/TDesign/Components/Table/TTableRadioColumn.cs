namespace TDesign;

/// <summary>
/// 表示呈现单选控件的列。
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class TTableRadioColumn<TItem> : TTableFieldColumn<TItem>
{
    [Parameter]public EventCallback<TTableRowSelectedEventArgs<TItem>> OnRowSelected { get; set; }

    protected override RenderFragment? GetColumnContent()
    {
        return HtmlHelper.CreateContent(builder =>
        {
            var valueType = Value?.GetType(); //获取数据类型

            var radioInputType= typeof(TInputRadio<>).MakeGenericType(valueType);
        });
    }
}
