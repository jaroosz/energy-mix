using EnergyMixApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ICarbonIntensityService, CarbonIntensityService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/getenergymixinfo", async (ICarbonIntensityService service) =>
{
    var result = await service.GetEnergyMixInfo();

    return Results.Ok(result);
});

app.MapGet("/getoptimalwindow", async (int hours, ICarbonIntensityService service) =>
{
    if (hours < 1 || hours > 6)
    {
        return Results.BadRequest("Hours parameter must be between 1 and 6.");
    }

    var result = await service.GetOptimalWindow(hours);

    return Results.Ok(result);
});

app.UseHttpsRedirection();

app.Run();
