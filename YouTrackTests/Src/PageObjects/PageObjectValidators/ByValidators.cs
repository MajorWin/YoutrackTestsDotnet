using System;
using Microsoft.VisualBasic.CompilerServices;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Extensions;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators
{
    public static class ByValidators
    {
        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        /// <exception cref="ElementNotVisibleException">If searchContext's Displayed property is false.</exception>
        public static Action ExistsAndDisplayedNow(By by, ISearchContext searchContext) =>
            () =>
            {
                var element = searchContext.FindElement(by);
                if (!element.Displayed)
                {
                    throw new ElementNotVisibleException($"{by} is not visible");
                }
            };

        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        /// <exception cref="ElementNotVisibleException">If searchContext's Displayed property is false.</exception>
        public static Action ExistsAndDisplayed(By by, ISearchContext searchContext, int timeoutSeconds = 5) =>
            () =>
            {
                var displayed = WaitHelpers.Wait(
                    () =>
                    {
                        try
                        {
                            return searchContext.FindElement(by).Displayed;
                        }
                        catch (StaleElementReferenceException) { }
                        catch (NoSuchElementException) { }

                        return false;
                    },
                    timeoutSeconds);

                // could throw NoSuchElementException
                searchContext.FindElement(by);

                if (!displayed)
                {
                    throw new ElementNotVisibleException($"{by} is not visible");
                }
            };
    }
}
