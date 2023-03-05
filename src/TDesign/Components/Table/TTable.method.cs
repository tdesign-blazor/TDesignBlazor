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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await QueryData(PageIndex, PageSize);
        }
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.CreateCascadingComponent(this, 0, base.BuildRenderTree, "Table");

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

        var (result, count) = await Data!.Query(page * size, (page - 1) * size);
        if (result is not null)
        {
            TableData.Clear();
            TableData.AddRange(result);
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

        if (rowIndex < 0 || rowIndex > TableData.Count)
        {
            return Task.CompletedTask;
        }

        var item = TableData[rowIndex];

        var selectedItem = SelectedRows.SingleOrDefault(m => m.RowIndex == rowIndex);

        if (selectedItem is not null)//已经选择过
        {
            SelectedRows.Remove(selectedItem);
            OnRowSelected.InvokeAsync(selectedItem);
        }
        else // 没有选择过
        {
            selectedItem = new(item, rowIndex);

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
}