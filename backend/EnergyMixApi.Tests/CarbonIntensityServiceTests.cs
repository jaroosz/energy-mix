using Xunit;
using EnergyMixApi.Services;
using FluentAssertions;
using EnergyMixApi.Models;
using Moq;

namespace EnergyMixApi.Tests
{
    public class CarbonIntensityServiceTests
    {
        [Fact]
        public void IsCleanEnergy_ShouldReturnTrue_forCleanSources()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new CarbonIntensityService(httpClient);

            // Act
            var isBiomassClean = service.IsCleanEnergy("biomass");
            var isWindClean = service.IsCleanEnergy("wind");
            var isNuclearClean = service.IsCleanEnergy("nuclear");
            var isSolarClean = service.IsCleanEnergy("solar");
            var isHydroClean = service.IsCleanEnergy("hydro");

            // Assert
            isBiomassClean.Should().BeTrue();
            isWindClean.Should().BeTrue();
            isNuclearClean.Should().BeTrue();
            isSolarClean.Should().BeTrue();
            isHydroClean.Should().BeTrue();
        }

        [Fact]
        public void IsCleanEnergy_ShouldReturnFalse_forNonCleanSources()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new CarbonIntensityService(httpClient);

            // Act
            var isImportsClean = service.IsCleanEnergy("imports");
            var isCoalClean = service.IsCleanEnergy("coal");
            var isGasClean = service.IsCleanEnergy("gas");
            var isOtherClean = service.IsCleanEnergy("other");

            // Assert
            isImportsClean.Should().BeFalse();
            isCoalClean.Should().BeFalse();
            isGasClean.Should().BeFalse();
            isOtherClean.Should().BeFalse();
        }

        [Fact]
        public void CalculateCleanEnergyPercent_ShouldSumCleanSources()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new CarbonIntensityService(httpClient);

            var sources = new Dictionary<string, double>
            {
                { "biomass", 3.7 },
                { "coal", 0 },
                { "imports", 10.1 },
                { "gas", 9.7 },
                { "nuclear", 13.7 },
                { "other", 0 },
                { "hydro", 0 },
                { "solar", 0.4 },
                { "wind", 62.3 }
            };

            // Act
            var result = service.CalculateCleanEnergyPercent(sources);

            // Assert
            result.Should().Be(80.1);
        }

        [Fact]
        public void CalculateCleanEnergyPercent_ShouldReturnZero_WhenNoCleanSources()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new CarbonIntensityService(httpClient);

            var sources = new Dictionary<string, double>
            {
                { "coal", 0 },
                { "imports", 10.1 },
                { "gas", 9.7 },
                { "other", 0 },
                { "hydro", 0 },
            };

            // Act
            var result = service.CalculateCleanEnergyPercent(sources);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateAverageSourcesForDay_ShouldCalculateCorrectAverages()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new CarbonIntensityService(httpClient);

            var data = new List<GenerationData>
            {
                new GenerationData
                {
                    From = new DateTime(2026, 1, 1, 0, 0, 0),
                    To = new DateTime(2026, 1, 1, 0, 30, 0),
                    GenerationMix = new List<FuelSource>
                    {
                        new FuelSource { Fuel = "biomass", Perc = 25.0 },
                        new FuelSource { Fuel = "other", Perc = 5.0 },
                        new FuelSource { Fuel = "hydro", Perc = 70.0 },
                    }
                },
                new GenerationData
                {
                    From = new DateTime(2026, 1, 1, 0, 30, 0),
                    To = new DateTime(2026, 1, 1, 1, 0, 0),
                    GenerationMix = new List<FuelSource>
                    {
                        new FuelSource { Fuel = "biomass", Perc = 50.0 },
                        new FuelSource { Fuel = "other", Perc = 0.0 },
                        new FuelSource { Fuel = "hydro", Perc = 30.0 },
                    }
                },
                new GenerationData
                {
                    From = new DateTime(2026, 1, 1, 1, 0, 0),
                    To = new DateTime(2026, 1, 1, 1, 30, 0),
                    GenerationMix = new List<FuelSource>
                    {
                        new FuelSource { Fuel = "biomass", Perc = 15.0 },
                        new FuelSource { Fuel = "other", Perc = 40.0 },
                        new FuelSource { Fuel = "hydro", Perc = 35.0 },
                    }
                }
            };

            var grouped = data.GroupBy(d => d.From.Date).First();

            // Act
            var result = service.CalculateAverageSourcesForDay(grouped);

            result["biomass"].Should().Be(30.0);
            result["other"].Should().Be(15.0);
            result["hydro"].Should().Be(45.0);
        }
    }
}
