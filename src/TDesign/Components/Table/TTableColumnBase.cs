﻿using System.Diagnostics.CodeAnalysis;

namespace TDesign;

/// <summary>
/// 表示表格的单元格的基类。
/// </summary>
public abstract class TTableColumnBase : TDesignComponentBase
{
    /// <summary>
    /// 级联的 <see cref="TTable"/> 组件。
    /// </summary>
    [CascadingParameter(Name = "Table")] protected TTableBase CascadingTable { get; set; }
    /// <summary>
    /// 设置列标题。若设置了 <see cref="HeaderContent"/> 参数，则该参数无效。
    /// </summary>
    [Parameter] public string? Header { get; set; }
    /// <summary>
    /// 设置列标题部分的任意 UI 片段。
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    /// <summary>
    /// 设置标题部分的额外 CSS 类字符串。
    /// </summary>
    [Parameter]public string? HeaderClass { get; set; }

    /// <summary>
    /// 设置每一列的额外 CSS 类的字符串。
    /// </summary>
    [Parameter] public string? ColumnClass { get; set; }

    /// <summary>
    /// 设置底部的任意 UI 片段。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }

    protected override void AfterSetParameters(ParameterView parameters)
    {
        base.AfterSetParameters(parameters);

        HeaderContent ??= builder => builder.AddContent(0, Header);
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if ( CascadingTable is null )
        {
            throw new InvalidOperationException("列必须定义在 TTable 组件中");
        }

        Header ??= $"标题{CascadingTable.ChildComponents.Count}";

        //任何时候，Header 必须有个值
        if ( !CascadingTable!.GetColumns().Any(m => m.Header!.Equals(Header)) )
        {
            CascadingTable.AddChildComponent(this);
        }
        base.OnInitialized();
    }
    /// <summary>
    /// 获取顶部标题内容。
    /// </summary>
    /// <returns></returns>
    internal virtual RenderFragment? GetHeaderContent() => HeaderContent;

    /// <summary>
    /// 获取表格列的内容。
    /// </summary>
    /// <param name="args">参数数组。</param>
    internal abstract RenderFragment? GetColumnContent(params object[]? args);
    /// <summary>
    /// 什么都不干。
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        //全部都在 TTable 里来创建列
    }


}
