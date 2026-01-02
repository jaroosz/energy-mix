namespace EnergyMixApi.Models
{
    /// <summary>
    /// Response containing energy mix data for multiple days
    /// </summary>
    public class EnergyMixResponse
    {
        public List<DailyEnergyData> Days { get; set; }
    }

    /// <summary>
    /// Energy data for a single day with averages per energy source and total clean energy percentage
    /// </summary>
    public class DailyEnergyData
    {
        public DateTime Date { get; set; }
        public Dictionary<string, double> Sources { get; set; }
        public double CleanEnergyPercent { get; set; }
    }
}
