using System;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.SeleniumUtilities;

namespace YouTrackWebdriverTests
{
    public static class TestEnvironment
    {
        public const string RootLogin = "root";
        public const string Password = "password";

        public static readonly string YoutrackAddress;
        public static readonly WebDriverInstantiator WebDriverInstantiator;
        public static readonly IWebDriver Browser;

        private const string DefaultYoutrackAddress = "http://localhost:8080";

        
        static TestEnvironment()
        {
            YoutrackAddress = TestContext.Parameters.Get("YoutrackAddress", DefaultYoutrackAddress);

            var browserTypeString = TestContext.Parameters.Get("Browser", BrowserType.Chrome.ToString());
            var browserType = Enum.Parse<BrowserType>(browserTypeString, true);

            WebDriverInstantiator = new WebDriverInstantiator(browserType);
            Browser = WebDriverInstantiator.CreateWebDriver();
        }

        public static string GetFullAddress(string absolutePath) => YoutrackAddress + absolutePath;
    }
}
