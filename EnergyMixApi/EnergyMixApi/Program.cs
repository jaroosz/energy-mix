using EnergyMixApi.Services;
using EnergyMixApi.Constants;
using EnergyMixApi.Middleware;

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
    var result = await service.GetEnergyMixInfo();
    return Results.Ok(result);
});

app.MapGet(ApiConstants.OptimalWindowRoute, async (int hours, ICarbonIntensityService service) =>
{
    if (hours < 1 || hours > 6)
    {
        throw new ArgumentException("Hours parameter must be between 1 and 6.");
    }

    var result = await service.GetOptimalWindow(hours);
    return Results.Ok(result);
});

app.Run();
