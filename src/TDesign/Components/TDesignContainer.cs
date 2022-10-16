using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 用于动态渲染组件的容器。
/// </summary>
public class TDesignContainer : BlazorComponentBase
{
    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<MessageContainer>(0);
    }
}
