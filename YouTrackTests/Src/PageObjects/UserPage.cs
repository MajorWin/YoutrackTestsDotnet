using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects
{
    public class UserPage : YoutrackPageObject
    {
        public const string Path = "/user";

        public static readonly By ChangePasswordFormLocator = By.Id("id_l.U.ChangePasswordDialog.changePasswordDlg");


        public UserPage(IWebDriver browser) :
            base(browser, new UriPathMatchesValidator(browser, Path))
        {
        }

        // [LogAspect]
        public IWebElement WaitForChangePasswordForm()
        {
            var (result, element) = WaitHelpers.WaitForResult(() =>
                Browser.TryFindElement(ChangePasswordFormLocator) is var form ? (true, form) : (false, null));

            if (!result)
                Browser.FindElement(ChangePasswordFormLocator); // to throw appropriate exception

            return element;
        }
    }
}