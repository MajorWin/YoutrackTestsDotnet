using System.Linq;
using System.Web;
using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class EditUserPage : YoutrackPageObject
    {
        public const string PathBase = "/editUser/";

        public EditUserPage(IWebDriver browser) :
            base(browser, new UriStartsWithValidator(browser, PathBase)) { }

        public string GetLoginFromUri() => HttpUtility.UrlDecode(GetUrlEncodedLoginFromUri());

        public string GetUrlEncodedLoginFromUri() => GetUri().Segments.Last();
    }
}
