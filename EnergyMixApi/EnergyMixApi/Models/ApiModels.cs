using System.Text.Json.Serialization;

namespace EnergyMixApi.Models
{
    /// <summary>
    /// Response from https://api.carbonintensity.org.uk/ API containing generation data
    /// </summary>
    public class CarbonIntensityApiResponse
    {
        public List<GenerationData> Data { get; set; }
    }

    /// <summary>
    /// Data for a 30-minute interval
    /// </summary>
    public class GenerationData
    {
        [JsonPropertyName("from")]
        public DateTime From { get; set; }

        [JsonPropertyName("to")]
        public DateTime To { get; set; }

        [JsonPropertyName("generationmix")]
        public List<FuelSource> GenerationMix { get; set; }
    }

    /// <summary>
    /// Fuel source with percentage share
    /// </summary>
    public class FuelSource
    {
        [JsonPropertyName("fuel")]
        public string Fuel { get; set; }

        [JsonPropertyName("perc")]
        public double Perc { get; set; }
    }
}
