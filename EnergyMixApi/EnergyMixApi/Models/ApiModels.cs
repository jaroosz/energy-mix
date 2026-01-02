using System.Text.Json.Serialization;

namespace EnergyMixApi.Models
{
    public class CarbonIntensityApiResponse
    {
        public List<GenerationData> Data { get; set; }
    }

    public class GenerationData
    {
        [JsonPropertyName("from")]
        public DateTime From { get; set; }
        [JsonPropertyName("to")]
        public DateTime To { get; set; }
        [JsonPropertyName("generationmix")]
        public List<FuelSource> GenerationMix { get; set; }
    }

    public class FuelSource
    {
        [JsonPropertyName("fuel")]
        public string Fuel { get; set; }
        [JsonPropertyName("perc")]
        public double Perc { get; set; }
    }
}
