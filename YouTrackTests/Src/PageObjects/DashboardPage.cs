using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class DashboardPage : YoutrackPageObject
    {
        public const string Path = "/dashboard";

        public DashboardPage(IWebDriver browser) :
            base(browser, new UriPathMatchesValidator(browser, Path))
        {
        }
    }
}