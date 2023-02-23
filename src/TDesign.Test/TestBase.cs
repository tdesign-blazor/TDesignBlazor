using Microsoft.Extensions.DependencyInjection;

namespace TDesign.Test;

/// <summary>
/// 测试基类。
/// </summary>
public class TestBase
{
    private readonly ServiceProvider _provider;
    private readonly TestContext _testContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class.
    /// </summary>
    public TestBase()
    {
        _testContext = new TestContext();
        _testContext.Services.AddTDesign().AddComponentBuilder();
        _provider = _testContext.Services.BuildServiceProvider();
    }
    /// <summary>
    /// 获取测试上下文对象。
    /// </summary>
    protected TestContext TestContext => _testContext;
    /// <summary>
    /// 获取注入的服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <returns>服务实例或 null。</returns>
    protected TService? GetService<TService>() => _provider.GetService<TService>();
}
/// <summary>
/// 测试基类、
/// </summary>
/// <typeparam name="TComponent">组件类型。</typeparam>
public class TestBase<TComponent> : TestBase where TComponent : IComponent
{
    /// <summary>
    /// 获取组件。
    /// </summary>
    /// <param name="parameterBuilder">参数构造器。</param>
    protected IRenderedComponent<TComponent> RenderComponent(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder) => TestContext.RenderComponent(parameterBuilder);

    /// <summary>
    /// 获取组件。
    /// </summary>
    /// <param name="parameters">组件参数数组。</param>
    protected IRenderedComponent<TComponent> RenderComponent(params ComponentParameter[] parameters) => TestContext.RenderComponent<TComponent>(parameters);

}