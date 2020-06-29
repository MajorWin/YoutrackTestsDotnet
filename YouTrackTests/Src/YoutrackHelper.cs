using OpenQA.Selenium;
using YouTrackWebdriverTests.Exceptions;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.PageObjects;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests
{
    public static class YoutrackHelper
    {
        public static void InitialSetupAndLoginAsRoot(IWebDriver browser)
        {
            browser.Navigate().GoToUrl(TestEnvironment.YoutrackAddress);

            var actualUri = browser.GetUri();

            switch (actualUri.AbsolutePath)
            {
                case YoutrackSettingsPage.AbsolutePath:
                    var settings = new YoutrackSettingsPage(browser);
                    settings.InitialSetup(TestEnvironment.RootLogin, TestEnvironment.Password);
                    break;
                case LoginPage.AbsolutePath:
                    new LoginPage(browser).LoginSuccessfully(TestEnvironment.RootLogin, TestEnvironment.Password);
                    break;
                default:
                    throw new WrongUrlException(
                        $"{actualUri.Authority}{YoutrackSettingsPage.AbsolutePath} or {actualUri.Authority}{LoginPage.AbsolutePath}",
                        $"{actualUri.Authority}{actualUri.AbsolutePath}");
            }
        }
    }
}
