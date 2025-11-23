using Microsoft.AspNetCore.Mvc;
using RIT.Contracts.Interfaces;
using RIT.Contracts.Models.Requests;

namespace RIT.WebApi.Endpoints;

public static partial class Endpoints
{
    public static IEndpointRouteBuilder RegisterAssetsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var mapGroup = endpoints.MapGroup("/api/assets").WithTags("Assets");

        mapGroup.MapGet("/", async (
            [FromServices] IAssetService service
            ) =>
        {
            var result = await service.GetAllAssetsAsync();

            return Results.Ok(result);
        });

        mapGroup.MapPost("/monetary", async (
            [FromBody] CreateMonetaryAssetRequest request,
            [FromServices] IAssetService service
            ) =>
        {
            var result = await service.CreateMonetaryAssetAsync(request);

            return Results.Created($"/assets/{result.Id}", result);
        });

        mapGroup.MapPost("/non-monetary", async (
            [FromBody] CreateNonMonetaryAssetRequest request,
            [FromServices] IAssetService service
            ) =>
        {
            var result = await service.CreateNonMonetaryAssetAsync(request);

            return Results.Created($"/assets/{result.Id}", result);
        });

        mapGroup.MapPut("/monetary/{id}", async (
            int id,
            [FromBody] CreateMonetaryAssetRequest request,
            [FromServices] IAssetService service
            ) =>
        {
            var result = await service.UpdateMonetaryAssetAsync(id, request);

            return Results.Created($"/assets/{result.Id}", result);
        });

        mapGroup.MapPut("/non-monetary/{id}", async (
            int id,
            [FromBody] CreateNonMonetaryAssetRequest request,
            [FromServices] IAssetService service
            ) =>
        {
            var result = await service.UpdateNonMonetaryAssetAsync(id, request);

            return Results.Created($"/assets/{result.Id}", result);
        });

        mapGroup.MapDelete("/{id}", async (
            int id,
            [FromServices] IAssetService service
            ) =>
        {
            var success = await service.DeleteAssetAsync(id);

            return success
                ? Results.NoContent()
                : Results.NotFound();
        });

        return endpoints;
    }
}
