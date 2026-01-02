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

    return result;
});

app.MapGet("/getoptimalwindow", async (int hours, ICarbonIntensityService service) =>
{
    var result = await service.GetOptimalWindow(hours);

    return result;
});

app.UseHttpsRedirection();

app.Run();
