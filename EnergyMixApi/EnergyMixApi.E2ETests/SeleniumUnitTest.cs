using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Xunit;

namespace EnergyMinApi.E2ETests
{
    public class HomePageTests
    {
        [Fact]
        public void Title_ShouldContainReactApp_WhenHomePageIsOpened()
        {
            // Arrange
            using IWebDriver driver = new EdgeDriver();

            // Act
            driver.Navigate().GoToUrl("https://energy-mix-frontend-53hj.onrender.com");

            // Assert
            Assert.Contains("React App", driver.Title);
        }
    }
}