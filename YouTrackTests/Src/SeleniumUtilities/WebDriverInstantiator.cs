using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace YouTrackWebdriverTests.SeleniumUtilities
{

    public class WebDriverInstantiator
    {
        private readonly BrowserType myBrowserType;

        public WebDriverInstantiator(BrowserType browserType)
        {
            myBrowserType = browserType;
        }

        public IWebDriver CreateWebDriver()
        {
            return myBrowserType switch
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
