namespace EnergyMixApi.Models
{
    /// <summary>
    /// Result of an optimal time window calculation for EV charging with clean energy percentage
    /// </summary>
    public class OptimalWindowResponse
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double CleanEnergyPercent { get; set; }
    }
}
