using RIT.WebApi.Endpoints;

namespace RIT.WebApi.BuilderExtensions;

public static class EndpointBuilder
{
    public static void UseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.RegisterAssetsEndpoints();
    }
}
