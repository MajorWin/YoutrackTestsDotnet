using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.UsersPageNamespace
{
    public class UserTableRow : PageObject
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

        private readonly By myRowLocator;


        public UserTableRow(IWebDriver browser, IWebElement userTr) :
            base(browser, WebElementValidators.OfATag("tr", userTr))
        {
            var loginLinkId = userTr.FindElement(LoginLocator).GetAttribute("id");
            myRowLocator = GetUserTrLocator(loginLinkId);
        }

        private UserTableRow(IWebDriver browser, By rowLocator) :
            base(browser, ByValidators.ExistsAndDisplayedNow(rowLocator, browser))
        {
            myRowLocator = rowLocator;
        }


        public bool IsOnline => UserTr.FindElement(OnlineStatusLocator).GetAttribute("class").Contains("user-online");

        public string Login => UserTr.FindElement(LoginLocator).Text;
        public string LoginTitle => UserTr.FindElement(LoginLocator).GetAttribute("title");
        public Uri LoginLink => new Uri(UserTr.FindElement(LoginLocator).GetAttribute("href"));

        public bool IsBanned => UserTr.TryFindElement(BannedLocator)?.Text == "banned";

        public string FullName => UserTr.FindElement(FullNameLocator).Text;
        public string FullNameTitle => UserTr.FindElement(FullNameLocator).GetAttribute("title");

        public string Email => UserTr.FindElement(EmailLocator).Text;
        public string EmailTitle => UserTr.FindElement(EmailLocator).GetAttribute("title");

        public string Jabber => UserTr.FindElement(JabberLocator).Text;
        public string JabberTitle => UserTr.FindElement(JabberLocator).GetAttribute("title");

        public IEnumerable<string> Groups => UserTr.FindElements(GroupLocator).Select(a => a.Text);

        public string LastAccess => UserTr.FindElement(LastAccessLocator).Text;


        private IWebElement UserTr => FindElement(myRowLocator);


        [LogAspect]
        public static UserTableRow GetRowByLogin(string login, IWebElement userTable, IWebDriver browser)
        {
            WebElementValidators.OfATag("table", userTable).Invoke();

            var loginLinkId = userTable
                .FindElements(LoginLocator)
                .First(a => a.Text == login)
                .GetAttribute("id");
            var rowLocator = GetUserTrLocator(loginLinkId);

            return new UserTableRow(browser, rowLocator);
        }


        [LogAspect]
        public UserTableRow ClickDeleteUser()
        {
            UserTr.FindElement(DeleteLinkLocator).Click();
            return this;
        }

        [LogAspect]
        public UserTableRow ClickMergeUser()
        {
            UserTr.FindElement(MergeLinkLocator).Click();
            return this;
        }

        [LogAspect]
        public UserTableRow ClickBanUser()
        {
            var banLink = UserTr.FindElement(BanLinkLocator);
            if (banLink.Text != "Ban")
            {
                throw new InvalidOperationException($"Can't click ban link. User {Login} already banned");
            }

            banLink.Click();
            return this;
        }

        [LogAspect]
        public UserTableRow ClickUnbanUser()
        {
            var unbanLink = UserTr.FindElement(BanLinkLocator);
            if (unbanLink.Text != "Unban")
            {
                throw new InvalidOperationException($"Can't click unban link. User {Login} already unbanned");
            }

            unbanLink.Click();
            return this;
        }


        [LogAspect]
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

        [LogAspect]
        public UserTableRow BanUser()
        {
            ClickBanUser();

            WaitHelpers.WaitIgnoringException<StaleElementReferenceException>(() => IsBanned);

            return this;
        }

        [LogAspect]
        public UserTableRow UnbanUser()
        {
            ClickUnbanUser();

            WaitHelpers.WaitIgnoringException<StaleElementReferenceException>(() => !IsBanned);

            return this;
        }


        private static By GetUserTrLocator(string loginLinkId) => By.XPath($"//a[@id = \"{loginLinkId}\"]/../..");
    }
}
