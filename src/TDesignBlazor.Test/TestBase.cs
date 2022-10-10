using ComponentBuilder;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace TDesignBlazor.Test;

/// <summary>
/// 测试基类。
/// </summary>
public class TestBase
{
    private readonly ServiceProvider _provider;
    private readonly TestContext _testContext;

    public TestBase()
    {

        _testContext = new TestContext();
        _testContext.Services.AddTDesignBlazor().AddComponentBuilder();
        _provider = _testContext.Services.BuildServiceProvider();
    }

    protected TestContext TestContext => _testContext;

    protected TService? GetService<TService>() => _provider.GetService<TService>();
}

public class TestBase<TComponent> : TestBase where TComponent : IComponent
{
    protected IRenderedComponent<TComponent> GetComponent(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder) => TestContext.RenderComponent(parameterBuilder);

    protected IRenderedComponent<TComponent> GetComponent(params ComponentParameter[] parameters) => TestContext.RenderComponent<TComponent>(parameters);

}