using TDesign.Abstractions;

namespace TDesign;

/// <summary>
/// 用于动态渲染组件的容器。
/// </summary>
public class TDesignContainer : ComponentBase
{
    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var index = 0;
        this.GetType().Assembly.GetTypes()
            .Where(m => typeof(IContainerComonent).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract)

            .ToList().ForEach(type =>
            {
                builder.CreateComponent(type, index);
                index++;
            });
    }
}
