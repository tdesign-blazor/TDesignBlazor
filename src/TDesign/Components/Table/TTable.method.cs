/*
 * TTable 的部分类，该cs文件用于对方法的归类
 */

namespace TDesign;

partial class TTable<TItem>
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

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.CreateCascadingComponent(this, 0, base.BuildRenderTree, "GenericTable");

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildTable(builder, sequence + 1);
        BuildPagination(builder, sequence + 2);
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
        await this.Refresh();

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
    /// 选中指定索引的行，如果该行被选中，则会取消选中。 <see cref="RowSelection"/> 为 <c>true</c> 时有效。
    /// </summary>
    /// <param name="rowIndex">要选择的行索引。</param>
    /// <exception cref="TDesignComponentException">无法找到指定行索引的数据。</exception>
    public Task SelectRow(int rowIndex)
    {
        if (!RowSelection)
        {
            return Task.CompletedTask;
        }

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
    public Task ExpandRow(int rowIndex)
    {
        if ( !GetColumns<TTableExpandColumn>().Any() )
        {
            return Task.CompletedTask;
        }
        var nextIndex = rowIndex + 1;
        if ( TryGetRowData(nextIndex, out _) )
        {
            TableData.RemoveAt(nextIndex);
        }
        else
        {
            TableData.Insert(nextIndex, new(TableRowDataType.Expand, default));
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
    private (TableRowDataType type, TItem? data) GetRowData(int rowIndex)
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
    private bool TryGetRowData(int rowIndex, out (TableRowDataType type, TItem? data) rowData)
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
}