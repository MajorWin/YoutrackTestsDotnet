using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.Base;
// using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.CommonObjects
{
    // It is enough for my task to assume that popups show errors only
    public class ErrorPopup : ValidatedWebObject
    {
        //TODO: there could be __popup__2, 3,..
        private static readonly By ErrorPopupLocator = By.CssSelector("div#__popup__1.message.error");
        private static readonly By ErrorMessagesLocator = By.CssSelector("td:nth-child(2) li");
        private static readonly By CloseLinkLocator = By.CssSelector("a.close");

        private readonly IWebElement _errorPopup;

        public ErrorPopup(IWebDriver browser) :
            base(browser, new WebElementExistsAndDisplayedValidator(browser, ErrorPopupLocator))
        {
            _errorPopup = browser.FindElement(ErrorPopupLocator);
        }

        public IEnumerable<string> ErrorMessages => _errorPopup.FindElements(ErrorMessagesLocator).Select(li => li.Text);
        public IEnumerable<string> NonEmptyErrorMessages => ErrorMessages.Where(message => message != "");

        // [LogAspect]
        public void Close()
        {
            _errorPopup.FindElement(CloseLinkLocator).Click();
            WaitHelpers.Wait(() => !Browser.IsElementDisplayed(ErrorPopupLocator));
        }
    }
}
