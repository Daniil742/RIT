using RIT.Database.Context;
using RIT.WebApi.BuilderExtensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceRegister();

builder.Services.AddCors(options =>
{
    options.AddPolicy("RequestPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:50350");
        policy.AllowCredentials();
        policy.WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS");
        policy.AllowAnyHeader();
    });
});

builder.AddDbContext();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AssetsDbContext>();

    db.Database.EnsureCreated();

    if (!db.Assets.Any())
    {
        db.Assets.AddRange(InitialData.InitAssets());
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("RequestPolicy");
app.UseEndpoints();

app.Run();
