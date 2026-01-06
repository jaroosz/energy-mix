using EnergyMixApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ICarbonIntensityService, CarbonIntensityService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("https://energy-mix-frontend-53hj.onrender.com", "http://localhost:3000");
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.MapGet("/api/energy-mix", async (ICarbonIntensityService service) =>
{
    try
    {
        var result = await service.GetEnergyMixInfo();

        return Results.Ok(result);
    }
    catch (HttpRequestException ex)
    {
        return Results.Problem("Failed to fetch data from external API");
    }
});

app.MapGet("/api/optimal-window", async (int hours, ICarbonIntensityService service) =>
{
    if (hours < 1 || hours > 6)
    {
        return Results.BadRequest("Hours parameter must be between 1 and 6.");
    }

    try
    {
        var result = await service.GetOptimalWindow(hours);

        return Results.Ok(result);
    }
    catch (HttpRequestException ex)
    {
        return Results.Problem("Failed to fetch data from external API");
    }
});

app.Run();
