using System;
using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects;
using YouTrackWebdriverTests.PageObjects.UsersPageNamespace;

namespace YouTrackWebdriverTests.SeleniumUtilities.Extensions
{
    public static class IWebDriverExtensions
    {
        public static Uri GetUri(this IWebDriver driver) => new Uri(driver.Url);

        public static UsersPage GoToUsersPage(this IWebDriver driver)
        {
            driver.Navigate().GoToUrl(TestEnvironment.GetFullAddress(UsersPage.Path));
            return new UsersPage(driver);
        }

        public static LoginPage GoToLoginPage(this IWebDriver driver)
        {
            driver.Navigate().GoToUrl(TestEnvironment.GetFullAddress(LoginPage.Path));
            return new LoginPage(driver);
        }
    }
}
