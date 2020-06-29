using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.CommonObjects;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects
{
    public abstract class PageObject
    {
        protected readonly IWebDriver Browser;

        protected PageObject(IWebDriver browser, Action validatePageObject)
        {
            Browser = browser;

            validatePageObject();
        }


        public Uri Uri => Browser.GetUri();

        [LogAspect]
        public ErrorPopup GetErrorPopup() => new ErrorPopup(Browser);


        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        protected IWebElement FindElement(By by) => Browser.FindElement(by);

        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        protected IWebElement FindElementWithWait(By by) => Browser.FindElementWithWait(by);

        protected IWebElement TryFindElement(By by) => Browser.TryFindElement(by);

        protected ReadOnlyCollection<IWebElement> FindElements(By by) => Browser.FindElements(by);

    }
}
