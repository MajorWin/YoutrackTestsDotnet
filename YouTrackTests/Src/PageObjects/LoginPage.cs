using OpenQA.Selenium;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class LoginPage : PageObject
    {
        public const string AbsolutePath = "/login";

        private static readonly By LoginLocator = By.Id("id_l.L.login");
        private static readonly By PasswordLocator = By.Id("id_l.L.password");
        private static readonly By LoginButtonLocator = By.Id("id_l.L.loginButton");


        public LoginPage(IWebDriver browser) : base(browser, UriValidators.Equals(AbsolutePath, browser)) { }


        [LogAspect]
        public DashboardPage LoginSuccessfully(string login, string password)
        {
            FindElement(LoginLocator).SendKeys(login);
            FindElement(PasswordLocator).SendKeys(password);
            FindElement(LoginButtonLocator).Click();

            return new DashboardPage(Browser);
        }

        [LogAspect]
        public UserPage LoginByForcedToChangePasswordUserSuccessfully(string login, string password)
        {
            FindElement(LoginLocator).SendKeys(login);
            FindElement(PasswordLocator).SendKeys(password);
            FindElement(LoginButtonLocator).Click();

            return new UserPage(Browser);
        }
    }
}
