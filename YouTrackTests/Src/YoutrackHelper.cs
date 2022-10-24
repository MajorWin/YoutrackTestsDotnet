using OpenQA.Selenium;
using YouTrackWebdriverTests.Exceptions;
using YouTrackWebdriverTests.Model;
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
                case YoutrackSettingsPage.Path:
                    var settings = new YoutrackSettingsPage(browser);
                    settings.InitialSetup(Configuration.Login, Configuration.Password);
                    break;
                case LoginPage.Path:
                    new LoginPage(browser).LoginSuccessfully(Configuration.Login, Configuration.Password);
                    break;
                default:
                    throw new WrongUrlException(
                        $"{actualUri.Authority}{YoutrackSettingsPage.Path} or {actualUri.Authority}{LoginPage.Path}",
                        $"{actualUri.Authority}{actualUri.AbsolutePath}");
            }
        }

        // [LogAspect]
        public static void CreateMaximumActiveUsers()
        {
            var usersPage = TestEnvironment.Browser.GoToUsersPage();
            var userSlotsRemaining = Configuration.ActiveUsersAllowed - usersPage.CountActiveUsers();

            for (var i = 0; i < userSlotsRemaining; i++)
            {
                var user = UserCreator.CreateFilledUser();

                usersPage
                    .OpenUserCreationForm()
                    .Fill(user)
                    .SubmitAndOpenEditUserPage();
                usersPage = TestEnvironment.Browser.GoToUsersPage();
            }
        }
    }
}
