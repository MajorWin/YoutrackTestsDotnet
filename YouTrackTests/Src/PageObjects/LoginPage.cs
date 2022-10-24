using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class LoginPage : YoutrackPageObject
    {
        public const string Path = "/login";

        private static readonly By LoginLocator = By.Id("id_l.L.login");
        private static readonly By PasswordLocator = By.Id("id_l.L.password");
        private static readonly By LoginButtonLocator = By.Id("id_l.L.loginButton");


        public LoginPage(IWebDriver browser) :
            base(browser, new UriPathMatchesValidator(browser, Path))
        {
        }


        // [LogAspect]
        public DashboardPage LoginSuccessfully(string login, string password)
        {
            Browser.FindElement(LoginLocator).SendKeys(login);
            Browser.FindElement(PasswordLocator).SendKeys(password);
            Browser.FindElement(LoginButtonLocator).Click();

            return new DashboardPage(Browser);
        }

        // [LogAspect]
        public UserPage LoginByForcedToChangePasswordUserSuccessfully(string login, string password)
        {
            Browser.FindElement(LoginLocator).SendKeys(login);
            Browser.FindElement(PasswordLocator).SendKeys(password);
            Browser.FindElement(LoginButtonLocator).Click();

            return new UserPage(Browser);
        }
    }
}
