using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class ProjectsPage : PageObject
    {
        public const string AbsolutePath = "/projects";

        public ProjectsPage(IWebDriver browser) :
            base(browser, UriValidators.Equals(AbsolutePath, browser)) { }
    }
}
