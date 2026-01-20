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

        [Fact]
        public void Page_ShouldContainHeader_WhenHomePageIsOpened()
        {
            // Arrange
            using IWebDriver driver = new EdgeDriver();

            // Act
            driver.Navigate().GoToUrl("https://energy-mix-frontend-53hj.onrender.com");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            // Assert
            Assert.True(driver.FindElement(By.TagName("header")).Displayed);
        }

        [Fact]
        public void Page_ShouldContainMainContent_WhenHomePageIsOpened()
        {
            // Arrange
            using IWebDriver driver = new EdgeDriver();

            // Act
            driver.Navigate().GoToUrl("https://energy-mix-frontend-53hj.onrender.com");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            // Assert
            Assert.True(driver.FindElement(By.ClassName("main-content-container")).Displayed);
        }

        [Fact]
        public void Page_ShouldContainOptimalChargingWindowSection_WhenHomePageIsOpened()
        {
            // Arrange
            using IWebDriver driver = new EdgeDriver();

            // Act
            driver.Navigate().GoToUrl("https://energy-mix-frontend-53hj.onrender.com");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            // Assert
            Assert.True(driver.FindElement(By.ClassName("optimal-window-section")).Displayed);
        }

        [Fact]
        public void Page_ShouldContainFooter_WhenHomePageIsOpened()
        {
            // Arrange
            using IWebDriver driver = new EdgeDriver();

            // Act
            driver.Navigate().GoToUrl("https://energy-mix-frontend-53hj.onrender.com");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            // Assert
            Assert.True(driver.FindElement(By.TagName("footer")).Displayed);
        }

        [Fact]
        public void Page_ShouldContainThreeCharts_WhenHomePageIsOpened()
        {
            // Arrange
            using IWebDriver driver = new EdgeDriver();

            // Act
            driver.Navigate().GoToUrl("https://energy-mix-frontend-53hj.onrender.com");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            var cards = driver.FindElements(By.ClassName("energy-card"));

            // Assert
            Assert.Equal(3, cards.Count);
            Assert.All(cards, card => Assert.True(card.Displayed));
        }

        [Fact]
        public void OptimalWindow_ShouldReturnOptimalWindowInfo_WhenUserSendRequest()
        {
            // Arrange
            using IWebDriver driver = new EdgeDriver();

            // Act
            driver.Navigate().GoToUrl("https://energy-mix-frontend-53hj.onrender.com");
            driver.FindElement(By.ClassName("find-window-button")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Assert
            Assert.True(driver.FindElement(By.ClassName("optimal-window-result")).Displayed);
        }
    }
}