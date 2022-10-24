using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects
{
    public class ProjectsPage : YoutrackPageObject
    {
        public const string Path = "/projects";

        public ProjectsPage(IWebDriver browser) :
            base(browser, new UriPathMatchesValidator(browser, Path))
        {
        }
    }
}