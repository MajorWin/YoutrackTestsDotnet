using OpenQA.Selenium;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UsersPage : PageObject
    {
        public const string AbsolutePath = "/users";

        private static readonly By CreateUserLinkLocator = By.Id("id_l.U.createNewUser");


        public UsersPage(IWebDriver browser) : base(browser, UriValidators.Equals(AbsolutePath, browser))
        {
            UserTable = new UserTable(browser);
        }


        public UserTable UserTable { get; }


        [LogAspect]
        public UsersPage ClickCreateUser()
        {
            FindElement(CreateUserLinkLocator).Click();
            return this;
        }


        [LogAspect]
        public UserCreationForm OpenUserCreationForm()
        {
            ClickCreateUser();
            return new UserCreationForm(Browser, this);
        }
    }
}
