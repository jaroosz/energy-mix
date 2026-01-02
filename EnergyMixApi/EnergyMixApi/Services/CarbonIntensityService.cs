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
            throw new NotImplementedException();
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
            var url = $"/generation/{from:yyyy-MM-dd}T00:30Z/{to:yyyy-MM-dd}T00:00Z/";

            var response = await _httpClient.GetFromJsonAsync<CarbonIntensityApiResponse>(url);

            return response;
        }
    }
}
