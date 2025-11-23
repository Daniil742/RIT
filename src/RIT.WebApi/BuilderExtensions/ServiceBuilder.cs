using RIT.Infrastructure;

namespace RIT.WebApi.BuilderExtensions;

public static class ServiceBuilder
{
    public static IServiceCollection AddServiceRegister(this IServiceCollection serviceCollection)
    {
        DependencyInjection.ConfigureServices(serviceCollection);

        return serviceCollection;
    }
}
