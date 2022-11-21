using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;

/// <summary>
/// 固钉组件。
/// </summary>
public class TAffix : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 距离容器顶部达到指定距离后触发固定，默认值0 。
    /// </summary>
    [Parameter]public int OffsetBottom { get; set; }

    /// <summary>
    /// 距离容器底部达到指定距离后触发固定，默认值0 。
    /// </summary>
    [Parameter]public int OffsetTop { get; set; }

    /// <summary>
    /// 固钉定位层级，样式默认为 500 。
    /// </summary>
    [Parameter] public int ZIndex { get; set; } = 500;

    /// <summary>
    /// 指定滚动的容器。滚动容器不是body时，传入滚动容器的id。
    /// </summary>
    [Parameter]public string Container { get; set; }
}

