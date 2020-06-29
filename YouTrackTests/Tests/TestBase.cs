using OpenQA.Selenium;

namespace YouTrackWebdriverTests.Tests
{
    public class TestBase
    {
        protected static IWebDriver CreateNewSession() => TestEnvironment.WebDriverInstantiator.CreateWebDriver();
    }
}
