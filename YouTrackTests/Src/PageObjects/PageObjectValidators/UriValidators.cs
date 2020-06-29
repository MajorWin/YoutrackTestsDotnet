using System;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Exceptions;
using YouTrackWebdriverTests.Extensions;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators
{
    public static class UriValidators
    {
        /// <exception cref="WrongUrlException">browser's current url's path does not match with absolutePath.</exception>
        public static Action Equals(string absolutePath, IWebDriver browser, int timeoutSeconds = 5) =>
            () =>
            {
                Uri actualUri = null;
                var isUriValid = WaitHelpers.Wait(
                    () =>
                    {
                        actualUri = browser.GetUri();
                        return actualUri.AbsolutePath == absolutePath;
                    },
                    timeoutSeconds);
                if (!isUriValid)
                {
                    throw new WrongUrlException(
                        $"{actualUri.Authority}{absolutePath}",
                        $"{actualUri.Authority}{actualUri.AbsolutePath}");
                }
            };

        /// <exception cref="WrongUrlException">browser's current url's path does not start with absolutePathPart.</exception>
        public static Action StartsWith(string absolutePathPart, IWebDriver browser, int timeoutSeconds = 5) =>
            () =>
            {
                Uri actualUri = null;
                var isUriValid = WaitHelpers.Wait(
                    () =>
                    {
                        actualUri = browser.GetUri();
                        return actualUri.AbsolutePath.StartsWith(absolutePathPart);
                    },
                    timeoutSeconds);
                if (!isUriValid)
                {
                    throw new WrongUrlException(
                        $"{actualUri.Authority}{absolutePathPart}...",
                        $"{actualUri.Authority}{actualUri.AbsolutePath}"
                    );
                }
            };
    }
}
