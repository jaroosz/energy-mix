namespace EnergyMixApi.Constants;

public static class FuelTypes
{
    public const string Biomass = "biomass";
    public const string Hydro = "hydro";
    public const string Nuclear = "nuclear";
    public const string Solar = "solar";
    public const string Wind = "wind";

    public static readonly string[] CleanSources =
    {
        Biomass, Hydro, Nuclear, Solar, Wind
    };
}
