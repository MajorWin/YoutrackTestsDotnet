using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UserTable : PageObject
    {
        private const string UserTableCssSelector = "div[id = 'id_l.U.usersList.usersList']";

        private static readonly By UserTableLocator = By.CssSelector($"{UserTableCssSelector} table");

        private static readonly By UserTableHeadLocator = By.CssSelector($"{UserTableCssSelector} thead tr");
        private static readonly By TotalUsersSpanLocator = By.CssSelector($"{UserTableCssSelector} th:nth-child(1) span");

        private static readonly By UserTrLocator = By.CssSelector($"{UserTableCssSelector} tbody tr");


        public UserTable(IWebDriver browser) :
            base(browser, ByValidators.ExistsAndDisplayedNow(UserTableLocator, browser)) { }


        public string TotalUsers => Browser.FindElement(TotalUsersSpanLocator).Text;

        [LogAspect]
        public IEnumerable<UserTableRow> UserRows => FindElements(UserTrLocator)
            .Select(row => new UserTableRow(Browser, row));

        private IWebElement UserTableElement => Browser.FindElement(UserTableLocator);


        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        [LogAspect]
        public UserTableRow FindRowByLogin(string login) => UserTableRow.GetRowByLogin(login, UserTableElement, Browser);

        [LogAspect]
        public void RemoveAllUsersExceptRootAndGuest()
        {
            foreach (var userToDelete in UserRows.ToList())
            {
                var login = userToDelete.Login;
                if (userToDelete.Login != "root" && userToDelete.Login != "guest")
                {
                    userToDelete.DeleteUser();
                }
            }
        }
    }
}
