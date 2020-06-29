using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class DashboardPage : PageObject
    {
        public const string AbsolutePath = "/dashboard";

        public DashboardPage(IWebDriver browser) :
            base(browser, UriValidators.Equals(AbsolutePath, browser)) { }
    }
}
