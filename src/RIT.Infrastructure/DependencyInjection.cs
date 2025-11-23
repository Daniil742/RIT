using Microsoft.Extensions.DependencyInjection;
using RIT.Contracts.Interfaces;
using RIT.Infrastructure.Services;

namespace RIT.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAssetService, AssetService>();

        return serviceCollection;
    }
}
