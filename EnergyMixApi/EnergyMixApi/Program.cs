using EnergyMixApi.Services;
using EnergyMixApi.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ICarbonIntensityService, CarbonIntensityService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(ApiConstants.CorsPolicyName, policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins(ApiConstants.ProductionFrontendUrl, ApiConstants.DevelopmentFrontendUrl);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseCors(ApiConstants.CorsPolicyName);
app.UseHttpsRedirection();

app.MapGet(ApiConstants.EnergyMixRoute, async (ICarbonIntensityService service) =>
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

app.MapGet(ApiConstants.OptimalWindowRoute, async (int hours, ICarbonIntensityService service) =>
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
