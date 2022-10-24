using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace YouTrackWebdriverTests.SeleniumUtilities
{

    public class WebDriverCreator
    {
        private readonly BrowserType _browserType;

        public WebDriverCreator(BrowserType browserType)
        {
            _browserType = browserType;
        }

        public IWebDriver CreateWebDriver()
        {
            return _browserType switch
            {
                BrowserType.Firefox => CreateFirefoxDriver(),
                BrowserType.Chrome => CreateChromeDriver(),
                _ => null
            };
        }

        private static IWebDriver CreateFirefoxDriver()
        {
            // https://github.com/SeleniumHQ/selenium/issues/7840
            var service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";

            return new FirefoxDriver(service);
        }

        private static IWebDriver CreateChromeDriver() => new ChromeDriver();
    }
}
