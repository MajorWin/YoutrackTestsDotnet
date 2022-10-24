using System.Linq;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Model;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UsersPage : YoutrackPageObject
    {
        public const string Path = "/users";

        private static readonly By CreateUserLinkLocator = By.Id("id_l.U.createNewUser");


        public UsersPage(IWebDriver browser) : base(browser, new UriPathMatchesValidator(browser, Path))
        {
            UserTable = new UserTable(browser);
        }


        public UserTable UserTable { get; }


        // [LogAspect]
        public UsersPage ClickCreateUser()
        {
            Browser.FindElement(CreateUserLinkLocator).Click();
            return this;
        }


        // [LogAspect]
        public UserCreationForm OpenUserCreationForm()
        {
            ClickCreateUser();
            return new UserCreationForm(Browser, this);
        }

        // [LogAspect]
        public bool IsFormClosed() =>
            Browser.IsElementDisplayed(UserCreationForm.UserCreationFormLocator);

        // [LogAspect]
        public int CountActiveUsers() =>
            UserTable
                .GetUserRows()
                .Count(userRow => !userRow.IsBanned);

        // [LogAspect]
        public void RemoveAllUsersExceptRootAndGuest()
        {
            // without .ToList() call we will get OpenQA.Selenium.StaleElementReferenceException
            var rows = UserTable.GetUserRows().ToList();

            foreach (var userToDelete in rows)
            {
                var login = userToDelete.Login;
                if (login != "root" && login != "guest")
                {
                    userToDelete.DeleteUser();
                }
            }
        }
    }
}
