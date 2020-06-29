using OpenQA.Selenium;

namespace YouTrackWebdriverTests.SeleniumUtilities.Extensions
{
    public static class ISearchContextExtensions
    {
        public static IWebElement TryFindElement(this ISearchContext searchContext, By by)
        {
            try
            {
                return searchContext.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        public static IWebElement FindElementWithWait(this ISearchContext searchContext, By by, int timeoutSeconds = 5)
        {
            WaitHelpers.Wait(() => searchContext.TryFindElement(by) != null, timeoutSeconds);

            return searchContext.FindElement(by);
        }

        public static bool IsElementDisplayed(this ISearchContext searchContext, By by)
        {
            try
            {
                return searchContext.FindElement(by).Displayed == false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }
    }
}
