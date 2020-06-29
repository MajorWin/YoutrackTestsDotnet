using OpenQA.Selenium;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class UserPage : PageObject
    {
        public const string AbsolutePath = "/user";

        public static readonly By ChangePasswordFormLocator = By.Id("id_l.U.ChangePasswordDialog.changePasswordDlg");


        public UserPage(IWebDriver browser) :
            base(browser, UriValidators.Equals(AbsolutePath, browser)) { }

        [LogAspect]
        public bool IsChangePasswordFormDisplayed() => TryFindElement(ChangePasswordFormLocator)?.Displayed ?? false;
    }
}
