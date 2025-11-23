using RIT.Database.Context;

namespace RIT.WebApi.BuilderExtensions;

public static class DbContextBuilder
{
    public static void AddDbContext(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContextFactory<AssetsDbContext>();
    }
}
