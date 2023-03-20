/*
 * TTable 的部分类，该cs文件用于对方法的归类
 */

namespace TDesign;

public partial class TTable<TItem>:TDesignComponentBase
{

    /// <inheritdoc/>
    protected override void AfterSetParameters(ParameterView parameters)
    {
        if (Data is null)
        {
            throw new InvalidOperationException($"必须提供 {nameof(Data)} 参数");
        }
        EmptyContent ??= builder => builder.AddContent(0, "暂无数据");
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QueryData(PageIndex, PageSize);
        }
    }


    /// <summary>
    /// 以异步的方式查询数据。
    /// </summary>
    /// <param name="page">页码。</param>
    /// <param name="size">数据量。</param>
    public async Task QueryData(int page = 1, int size = 10)
    {
        if (page < 1)
        {
            throw new InvalidOperationException("页码不能小于1");
        }
        if (size < 0)
        {
            throw new InvalidOperationException("数据量不能小于0");
        }

        DataLoadedComplete = false;
        Loading = true;
        StateHasChanged();

        var (data, count) = await Data!.Query(page * size, (page - 1) * size);
        if (data is not null)
        {
            TableData.Clear();
            AddDataRange(data);
        }
        TotalCount = count;

        Loading = false;

        PageIndex = page;
        PageSize = size;
        await ChangeTotalCount(TotalCount);

        DataLoadedComplete = true;
        await this.Refresh();
    }

    /// <summary>
    /// 变更总记录数。
    /// </summary>
    /// <param name="count">总记录数。</param>
    /// <exception cref="InvalidOperationException"><paramref name="count"/> 小于0。</exception>
    public Task ChangeTotalCount(int count)
    {
        if (count < 0)
        {
            throw new InvalidOperationException("总数不能小于0");
        }

        TotalCount = count;
        return TotalCountChanged.InvokeAsync(count);
    }

    #region 选择行
    /// <summary>
    /// 选中指定索引的行，如果该行被选中，则会取消选中。
    /// </summary>
    /// <param name="rowIndex">要选择的行索引。</param>
    /// <exception cref="TDesignComponentException">无法找到指定行索引的数据。</exception>
    public Task SelectRow(int rowIndex)
    {
        if (rowIndex < 0)
        {
            return Task.CompletedTask;
        }

        var selectedItem = SelectedRows.SingleOrDefault(m => m.RowIndex == rowIndex);

        if ( selectedItem is not null )//已经选择过
        {
            SelectedRows.Remove(selectedItem);
            OnRowSelected.InvokeAsync(selectedItem);
        }
        else // 没有选择过
        {
            if(!TryGetRowData(rowIndex, out var rowItem) )
            {
                //没有抓到数据，如何处理
                return Task.CompletedTask;
            }

            selectedItem = new(rowItem.data, rowIndex);

            if (IsSingleSelection)
            {
                SelectedRows.Clear();
            }
            SelectedRows.Add(selectedItem);
            OnRowSelected.InvokeAsync(selectedItem);
        }

        return this.Refresh();
    }
    #endregion

    #region 展开/收缩行
    /// <summary>
    /// 展开/收缩指定索引的行。
    /// </summary>
    /// <param name="rowIndex">行索引。</param>
    public Task ExpandRow(int rowIndex)
    {
        var expandColumn = GetColumns<TTableExpandColumn<TItem>>().FirstOrDefault();

        if ( expandColumn is null )
        {
            return Task.CompletedTask;
        }

        var nextIndex = rowIndex + 1;

        if ( TryGetRowData(rowIndex, out var row) )
        {
            try
            {
                var (type, data) = TableData[nextIndex];
                if ( type == TableRowDataType.Expand )
                {
                    TableData.RemoveAt(nextIndex);
                }
                else
                {
                    TableData.Insert(nextIndex, new(TableRowDataType.Expand, row.data));
                }
            }
            catch
            {
                TableData.Insert(nextIndex, new(TableRowDataType.Expand, row.data));
            }
        }

        return this.Refresh();
    }
    #endregion

    #region Private
    private void AddData(TableRowDataType rowType, TItem data) => TableData.Add((rowType, data));

    private void AddDataRange(IEnumerable<TItem> rows) => rows.ForEach(item => AddData(TableRowDataType.Data, item));

    /// <summary>
    /// 获取指定行索引的数据。
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    /// <exception cref="TDesignComponentException">指定行索引不存在。</exception>
    internal (TableRowDataType type, TItem? data) GetRowData(int rowIndex)
    {
        try
        {
            return TableData[rowIndex];

        }
        catch ( ArgumentOutOfRangeException ex )
        {
            throw new TDesignComponentException(this, $"指定的行索引{rowIndex}不存在", ex);
        }
    }

    /// <summary>
    /// 尝试获取指定行索引的数据。
    /// </summary>
    /// <param name="rowIndex">行的索引位置。</param>
    /// <param name="rowData">获取到的行数据。</param>
    /// <returns>当能获取到行数据时返回 <c>true</c>；否则返回 <c>false</c>。</returns>
    internal bool TryGetRowData(int rowIndex, out (TableRowDataType type, TItem? data) rowData)
    {
        try
        {
            rowData = GetRowData(rowIndex);
            return true;
        }
        catch
        {
            rowData = new(TableRowDataType.Unknow, default);
            return false;
        }
    }
    #endregion

    #region GetColumns
    /// <summary>
    /// 获取列集合。
    /// </summary>
    protected internal IEnumerable<TTableColumnBase<TItem>> GetColumns() => GetColumns<TTableColumnBase<TItem>>();
    /// <summary>
    /// 获取指定类型的列集合。
    /// </summary>
    /// <typeparam name="TTableColumn">列的类型。</typeparam>
    protected internal IEnumerable<TTableColumn> GetColumns<TTableColumn>() where TTableColumn : TTableColumnBase<TItem> => ChildComponents.OfType<TTableColumn>();
    #endregion

    #region 编辑

    /// <summary>
    /// 获取或创建可编辑状态。
    /// </summary>
    /// <param name="rowIndex">行索引。</param>
    /// <param name="editable">可否编辑。如果是 <c>null</c> 时，若 rowIndex 不存在，则默认状态时 <c>false</c>；若 rowIndex 存在，则更新为该值。</param>
    /// <returns><c>True</c> 表示编辑状态，否则为 <c>false</c>。</returns>
    internal bool GetOrCreateEditableState(int rowIndex, bool? editable = default)
    {
        if ( !EditableState.ContainsKey(rowIndex) )
        {
            EditableState.Add(rowIndex, editable ?? false);
        }
        if ( editable.HasValue )
        {
            EditableState[rowIndex] = editable.Value;
        }
        return EditableState[rowIndex];
    }


    /// <summary>
    /// 切换到编辑视图。
    /// </summary>
    /// <param name="rowIndex">行索引。</param>
    /// <param name="item">当前数据。</param>
    public Task SwitchToEditView(int rowIndex,TItem item)
    {
        GetOrCreateEditableState(rowIndex, true);
        OnRowEditing.InvokeAsync(new(item, rowIndex));
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 切换到非编辑视图。
    /// </summary>
    /// <param name="rowIndex">行索引。</param>
    /// <param name="item">当前数据。</param>
    public Task SwitchToNonEditView(int rowIndex,TItem item)
    {
        GetOrCreateEditableState(rowIndex, false);
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 提交保存指定行的数据。
    /// </summary>
    /// <param name="rowIndex">行索引。</param>
    /// <param name="item">变化的数据。</param>
    public async Task SubmitEditting(int rowIndex,TItem item)
    {
        await OnRowEdited.InvokeAsync(new(item, rowIndex));
        await SwitchToNonEditView(rowIndex, item);
    }
    #endregion
}