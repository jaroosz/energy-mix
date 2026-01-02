using System.Text.Json.Serialization;

namespace EnergyMixApi.Models
{
    public class CarbonIntensityApiResponse
    {
        public List<GenerationData> Data { get; set; }
    }

    public class GenerationData
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<FuelSource> GenerationMix { get; set; }
    }

    public class FuelSource
    {
        public string Fuel { get; set; }
        public double Perc { get; set; }
    }
}
