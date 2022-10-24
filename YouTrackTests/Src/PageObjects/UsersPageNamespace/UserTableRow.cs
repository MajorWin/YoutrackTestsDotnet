using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UserTableRow : YoutrackPageObject
    {
        private static readonly By OnlineStatusLocator = By.CssSelector(@"td:nth-child(1) span.user-status");
        private static readonly By LoginLocator = By.CssSelector(@"td:nth-child(1) a[id ^= 'id_l.U.usersList.UserLogin.editUser']");
        private static readonly By BannedLocator = By.CssSelector(@"td:nth-child(1) span.smallRed");

        private static readonly By FullNameLocator = By.CssSelector(@"td:nth-child(2) div");

        private static readonly By EmailLocator = By.CssSelector(@"td:nth-child(3) div:nth-child(1)");
        private static readonly By JabberLocator = By.CssSelector(@"td:nth-child(3) div:nth-child(2)");

        private static readonly By GroupLocator = By.CssSelector(@"td:nth-child(4) a");

        private static readonly By LastAccessLocator = By.CssSelector(@"td:nth-child(5)");

        private static readonly By DeleteLinkLocator = By.CssSelector(@"td:nth-child(6) a[id ^= 'id_l.U.usersList.deleteUser']");
        private static readonly By MergeLinkLocator = By.CssSelector(@"td:nth-child(6) a[id ^= 'id_l.U.usersList.mergeUser']");
        private static readonly By BanLinkLocator = By.CssSelector(@"td:nth-child(6) a[id ^= 'id_l.U.usersList.banUser']");

        // user table is updated dynamically so we can not store IWebElements
        private readonly By _rowLocator;


        public UserTableRow(IWebDriver browser, IWebElement rowElement) :
            base(browser, new TagValidator(rowElement, "tr"))
        {
            var loginLinkId = rowElement.FindElement(LoginLocator).GetAttribute("id");
            _rowLocator = GetRowLocator(loginLinkId);
        }

        private UserTableRow(IWebDriver browser, By rowLocator) :
            base(browser, new WebElementExistsAndDisplayedNowValidator(browser, rowLocator))
        {
            _rowLocator = rowLocator;
        }


        public bool IsOnline => GetRowElement().FindElement(OnlineStatusLocator).GetAttribute("class").Contains("user-online");

        public string Login => GetRowElement().FindElement(LoginLocator).Text;
        public string LoginTitle => GetRowElement().FindElement(LoginLocator).GetAttribute("title");
        public Uri LoginLink => new Uri(GetRowElement().FindElement(LoginLocator).GetAttribute("href"));

        public bool IsBanned => GetRowElement().TryFindElement(BannedLocator)?.Text == "banned";

        public string FullName => GetRowElement().FindElement(FullNameLocator).Text;
        public string FullNameTitle => GetRowElement().FindElement(FullNameLocator).GetAttribute("title");

        public string Email => GetRowElement().FindElement(EmailLocator).Text;
        public string EmailTitle => GetRowElement().FindElement(EmailLocator).GetAttribute("title");

        public string Jabber => GetRowElement().FindElement(JabberLocator).Text;
        public string JabberTitle => GetRowElement().FindElement(JabberLocator).GetAttribute("title");

        public IEnumerable<string> Groups => GetRowElement().FindElements(GroupLocator).Select(a => a.Text);

        public string LastAccess => GetRowElement().FindElement(LastAccessLocator).Text;


        public IWebElement GetRowElement() => Browser.FindElement(_rowLocator);

        // [LogAspect]
        public static UserTableRow GetRowByLogin(string login, IWebElement userTable, IWebDriver browser)
        {
            var loginLinkId = userTable
                .FindElements(LoginLocator)
                .First(a => a.Text == login)
                .GetAttribute("id");
            var rowLocator = GetRowLocator(loginLinkId);

            return new UserTableRow(browser, rowLocator);
        }

        // [LogAspect]
        public UserTableRow ClickDeleteUser()
        {
            GetRowElement().FindElement(DeleteLinkLocator).Click();
            return this;
        }

        // [LogAspect]
        public UserTableRow ClickMergeUser()
        {
            GetRowElement().FindElement(MergeLinkLocator).Click();
            return this;
        }

        // [LogAspect]
        public UserTableRow ClickBanUser()
        {
            var banLink = GetRowElement().FindElement(BanLinkLocator);
            if (banLink.Text != "Ban")
            {
                throw new InvalidOperationException($"Can't click ban link. User {Login} already banned");
            }

            banLink.Click();
            return this;
        }

        // [LogAspect]
        public UserTableRow ClickUnbanUser()
        {
            var unbanLink = GetRowElement().FindElement(BanLinkLocator);
            if (unbanLink.Text != "Unban")
            {
                throw new InvalidOperationException($"Can't click unban link. User {Login} already unbanned");
            }

            unbanLink.Click();
            return this;
        }


        // [LogAspect]
        public UsersPage DeleteUser()
        {
            ClickDeleteUser();

            WaitHelpers.WaitIgnoringException<NoAlertPresentException>(() =>
            {
                Browser.SwitchTo().Alert().Accept();
                return true;
            });

            // Need to sleep to wait for page to refresh or refresh it manually
            return Browser.GoToUsersPage();
        }

        // [LogAspect]
        public UserTableRow BanUser()
        {
            ClickBanUser();

            WaitHelpers.WaitIgnoringException<StaleElementReferenceException>(() => IsBanned);

            return this;
        }

        // [LogAspect]
        public UserTableRow UnbanUser()
        {
            ClickUnbanUser();

            WaitHelpers.WaitIgnoringException<StaleElementReferenceException>(() => !IsBanned);

            return this;
        }


        private static By GetRowLocator(string loginLinkId) => By.XPath($"//a[@id = \"{loginLinkId}\"]/../..");
    }
}
