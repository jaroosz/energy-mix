using EnergyMixApi.Models;
using EnergyMixApi.Constants;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EnergyMixApi.Tests")]

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
            _httpClient.BaseAddress = new Uri(ApiConstants.CarbonIntensityApiBaseUrl);
        }

        public async Task<EnergyMixResponse> GetEnergyMixInfo()
        {
            var from = DateTime.UtcNow.Date;
            var to = from.AddDays(3);

            var data = await FetchGenerationData(from, to);

            var groupedByDay = data.Data.GroupBy(d => d.From.Date);

            var dailyDataList = new List<DailyEnergyData>();

            // Calculate average and clean energy percentage for each day
            foreach (var day in groupedByDay)
            {
                var sources = CalculateAverageSourcesForDay(day);
                var cleanEnergyPercent = CalculateCleanEnergyPercent(sources);

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
            var from = DateTime.UtcNow.Date.AddDays(1);
            var to = from.AddDays(2);

            var data = await FetchGenerationData(from, to);

            var intervals = data.Data;
            var windowSize = hours * 2; // Each hour contain 2 intervals (30-minute intervals)

            // Find the window with highest clean energy percentage using sliding window technique
            double bestAverage = 0;
            int bestStartIndex = 0;

            for (int i = 0; i <= intervals.Count - windowSize; i++)
            {
                var window = intervals.Skip(i).Take(windowSize);

                var averageClean = window
                    .Select(interval => interval.GenerationMix
                        .Where(fuel => IsCleanEnergy(fuel.Fuel))
                        .Sum(fuel => fuel.Perc)
                    )
                    .Average();

                if (averageClean > bestAverage)
                {
                    bestAverage = averageClean;
                    bestStartIndex = i;
                }
            }

            var bestWindow = intervals.Skip(bestStartIndex).Take(windowSize).ToList();

            return new OptimalWindowResponse
            {
                StartTime = bestWindow.First().From,
                EndTime = bestWindow.Last().To,
                CleanEnergyPercent = bestAverage
            };
        }

        /// <summary>
        /// Compare fuel name with list of clean energy sources
        /// </summary>
        /// <param name="fuelName">Name of the fuel to check</param>
        /// <returns>True if fuel is clean energy source, otherwise false</returns>
        internal bool IsCleanEnergy(string fuelName)
        {
            return FuelTypes.CleanSources.Contains(fuelName);
        }

        /// <summary>
        /// Calculates average percentage contribution of each energy source for a given day
        /// </summary>
        /// <param name="day">Generation data for one day (48 intervals)</param>
        /// <returns>Dictionary with fuel names and their average percentage</returns>
        internal Dictionary<string, double> CalculateAverageSourcesForDay(IGrouping<DateTime, GenerationData> day)
        {
            return day
                .SelectMany(interval => interval.GenerationMix)
                .GroupBy(fuel => fuel.Fuel)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Average(x => x.Perc)
                );
        }

        /// <summary>
        /// Calculates total percentage of clean energy sources
        /// </summary>
        /// <param name="sources">Dictionary od energy sources with percentages</param>
        /// <returns>Sum of clean energy percentages</returns>
        internal double CalculateCleanEnergyPercent(Dictionary<string, double> sources)
        {
            return FuelTypes.CleanSources
                .Where(name => sources.ContainsKey(name))
                .Sum(name => sources[name]);
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
