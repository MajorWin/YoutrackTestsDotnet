using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Logging;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.CommonObjects
{
    // It is enough for my task to assume that popups show errors only
    public class ErrorPopup : PageObject
    {
        //TODO: there could be __popup__2, 3,..
        private static readonly By ErrorPopupLocator = By.CssSelector("div#__popup__1.message.error");
        private static readonly By ErrorMessagesLocator = By.CssSelector("td:nth-child(2) li");
        private static readonly By CloseLinkLocator = By.CssSelector("a.close");

        private readonly IWebElement myErrorPopup;

        public ErrorPopup(IWebDriver browser) :
            base(browser, ByValidators.ExistsAndDisplayed(ErrorPopupLocator, browser))
        {
            myErrorPopup = FindElement(ErrorPopupLocator);
        }

        public IEnumerable<string> ErrorMessages => myErrorPopup.FindElements(ErrorMessagesLocator).Select(li => li.Text);
        public IEnumerable<string> NonEmptyErrorMessages => ErrorMessages.Where(message => message != "");

        [LogAspect]
        public void Close()
        {
            myErrorPopup.FindElement(CloseLinkLocator).Click();
            WaitHelpers.Wait(() => !Browser.IsElementDisplayed(ErrorPopupLocator));
        }
    }
}
