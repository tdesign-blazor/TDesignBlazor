namespace TDesign;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class TTableOperationColumn<TItem> : TTableColumnBase<TItem>
{
    /// <summary>
    /// 设置一个委托，表示是否可编辑行。
    /// </summary>
    [Parameter] public Func<bool> Editable { get; set; }
    /// <summary>
    /// 设置在非编辑状态下，可编辑操作的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? EditableContent { get; set; }
    /// <summary>
    /// 设置一个委托，表示是否可移除行。
    /// </summary>
    [Parameter] public Func<bool> Removable { get; set; }
    /// <summary>
    /// 在非编辑状态下，可移除操作的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? RemovableContent { get; set; }
    /// <summary>
    /// 设置在编辑状态下，提交编辑操作的任意 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? SubmitContent { get; set; }
    /// <summary>
    /// 设置在编辑状态下，取消编辑操作的任意 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? CancelContent { get; set; }

    /// <inheritdoc/>
    protected override void AfterSetParameters(ParameterView parameters)
    {
        base.AfterSetParameters(parameters);

        Header ??= "操作";        

        Editable ??= () => true;
        Removable ??= () => true;

        EditableContent ??= builder => builder.Component<TLink>()
                                                .ChildContent("编辑")
                                                .Attribute(nameof(TLink.Theme), Theme.Primary)
                                                .Close();
        RemovableContent ??= builder => builder.Component<TLink>()
                                                .ChildContent("删除")
                                                .Attribute(nameof(TLink.Theme), Theme.Danger)
                                                .Close();

        SubmitContent ??= builder => builder.Component<TButton>().ChildContent("提交").Attribute(nameof(TButton.Theme), Theme.Primary).Close();
        CancelContent ??= builder => builder.Component<TButton>()
                                                    .ChildContent("取消")
                                                    .Attribute(nameof(TButton.Theme), Theme.Default)
                                                    .Close();
    }

    /// <inheritdoc/>
    protected internal override RenderFragment? GetCellContent(int rowIndex, TItem item)
    {

        if ( Table.GetOrCreateEditableState(rowIndex) )//编辑状态
        {
            return builder =>
            {
                builder.CreateComponent<TSpace>(0, content =>
                {
                    content.CreateComponent<TSpaceItem>(0, SubmitContent, new
                    {
                        onclick = HtmlHelper.Event.Create(this, e => Table.SubmitEditting(rowIndex, item))
                    });

                    content.CreateComponent<TSpaceItem>(1, CancelContent, new
                    {
                        @onclick = HtmlHelper.Event.Create(this, e => Table.SwitchToNonEditView(rowIndex, item))
                    });
                });
            };
        }
        else//非编辑状态
        {
            return builder =>
            {
                builder.CreateComponent<TSpace>(0, content =>
                {
                    if ( Editable() )
                    {
                        content.CreateComponent<TSpaceItem>(0, EditableContent, new
                        {
                            @onclick = HtmlHelper.Event.Create(this, e => Table.SwitchToEditView(rowIndex, item))
                        });
                    }

                    if ( Removable() )
                    {
                        content.CreateComponent<TSpaceItem>(1, RemovableContent, new
                        {
                            //@onclick = HtmlHelper.Event.Create(this, e => Table.SwitchToEditView(rowIndex))
                        });
                    }
                });
            };
        }
    }
}
