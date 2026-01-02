using EnergyMixApi.Models;

namespace EnergyMixApi.Services
{
    public class CarbonIntensityService : ICarbonIntensityService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes the service and sets base Url address
        /// </summary>
        /// <param name="httpClient"></param>
        public CarbonIntensityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.carbonintensity.org.uk");
        }

        public async Task<EnergyMixResponse> GetEnergyMixInfo()
        {
            var from = DateTime.UtcNow.Date;
            var to = from.AddDays(3);

            var data = await FetchGenerationData(from, to);

            var groupedByDay = data.Data.GroupBy(d => d.From.Date);

            var dailyDataList = new List<DailyEnergyData>();

            foreach (var day in groupedByDay)
            {
                var energySource = day
                    .SelectMany(interval => interval.GenerationMix)
                    .Select(fuel => fuel.Fuel)
                    .Distinct();

                var sources = new Dictionary<string, double>();

                foreach (var fuelName in energySource)
                {
                    var average = day
                        .SelectMany(interval => interval.GenerationMix)
                        .Where(fuel => fuel.Fuel == fuelName)
                        .Average(fuel => fuel.Perc);

                    sources.Add(fuelName, average);
                }

                var cleanEnergyPercent =
                    sources["biomass"] +
                    sources["nuclear"] +
                    sources["hydro"] +
                    sources["wind"] +
                    sources["solar"];

                var dailyEnergyData = new DailyEnergyData
                {
                    Date = day.Key,
                    Sources = sources,
                    CleanEnergyPercent = cleanEnergyPercent
                };

                dailyDataList.Add(dailyEnergyData);
            }

            return new EnergyMixResponse
                    {
                        Days = dailyDataList
                    };
        }

        public async Task<OptimalWindowResponse> GetOptimalWindow(int hours)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches generation mix data for the specified date range from the Carbon Intensity API
        /// </summary>
        /// <param name="from">The start date of the range for which to retrieve generation data</param>
        /// <param name="to">The end date of the range for which to retrieve generation data</param>
        private async Task<CarbonIntensityApiResponse> FetchGenerationData(DateTime from, DateTime to)
        {
            // starting at 00:30 to avoid getting last interval (23:30 - 00:00) from previous day 
            var url = $"/generation/{from:yyyy-MM-dd}T00:30Z/{to:yyyy-MM-dd}T00:00Z/";

            var response = await _httpClient.GetFromJsonAsync<CarbonIntensityApiResponse>(url);

            return response;
        }
    }
}
