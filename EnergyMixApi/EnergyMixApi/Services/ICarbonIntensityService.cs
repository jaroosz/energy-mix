using EnergyMixApi.Models;

namespace EnergyMixApi.Services
{
    /// <summary>
    /// Service for fetching energy generation data
    /// </summary>
    public interface ICarbonIntensityService
    {
        /// <summary>
        /// Gets energy mix data for three consecutive days, starting from the current day
        /// </summary>
        /// <returns>Energy mix info with daily average</returns>
        Task<EnergyMixResponse> GetEnergyMixInfo();

        /// <summary>
        /// Finds charging window with highest clean energy percentage
        /// </summary>
        /// <param name="hours">Charging duration in hours (1-6)</param>
        /// <returns>Optimal charging window with start, end time and average clean energy percentage</returns>
        Task<OptimalWindowResponse> GetOptimalWindow(int hours);
    }
}