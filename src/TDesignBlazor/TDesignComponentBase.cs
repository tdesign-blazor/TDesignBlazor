using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace TDesignBlazor;
public abstract class TDesignComponentBase : BlazorComponentBase
{
    protected TDesignOptions Options => ServiceProvider.GetRequiredService<IOptions<TDesignOptions>>().Value;
}
