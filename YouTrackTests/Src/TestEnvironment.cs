using System;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.SeleniumUtilities;

namespace YouTrackWebdriverTests
{
    public static class TestEnvironment
    {
        public static readonly Uri YoutrackAddress;
        public static readonly WebDriverCreator WebDriverCreator;
        public static readonly IWebDriver Browser;


        static TestEnvironment()
        {
            var uriString = TestContext.Parameters.Get("YoutrackAddress", Configuration.YoutrackAddress);
            YoutrackAddress = new Uri(uriString);

            var browserTypeString = TestContext.Parameters.Get("Browser", BrowserType.Chrome.ToString());
            var browserType = Enum.Parse<BrowserType>(browserTypeString, true);

            WebDriverCreator = new WebDriverCreator(browserType);
            Browser = WebDriverCreator.CreateWebDriver();
        }

        public static Uri GetFullAddress(string path) => new(YoutrackAddress, path);
    }
}