using System.Linq;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects
{
    public class EditUserPage : PageObject
    {
        public const string AbsolutePathBase = "/editUser/";

        public EditUserPage(IWebDriver browser) :
            base(browser, UriValidators.StartsWith(AbsolutePathBase, browser)) { }

        public string LoginFromUri => Browser.GetUri().Segments.Last();
    }
}
