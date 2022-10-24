using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UserTable : YoutrackPageObject
    {
        private static readonly By UserTableLocator = By.CssSelector($"div[id = 'id_l.U.usersList.usersList'] table");

        private static readonly By UserTableHeadLocator = By.CssSelector($"thead tr");
        private static readonly By TotalUsersSpanLocator = By.CssSelector($"th:nth-child(1) span");

        private static readonly By RowLocator = By.CssSelector($"tbody tr");

        private readonly IWebElement _tableElement;


        public UserTable(IWebDriver browser) :
            base(browser, new WebElementExistsAndDisplayedNowValidator(browser, UserTableLocator))
        {
            _tableElement = Browser.FindElement(UserTableLocator);
        }


        public string TotalUsers => _tableElement.FindElement(TotalUsersSpanLocator).Text;


        // [LogAspect]
        public IEnumerable<UserTableRow> GetUserRows() => _tableElement.FindElements(RowLocator)
            .Select(row => new UserTableRow(Browser, row));

        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">No element matches the criteria.</exception>
        // [LogAspect]
        public UserTableRow FindRowByLogin(string login) => UserTableRow.GetRowByLogin(login, _tableElement, Browser);
    }
}
