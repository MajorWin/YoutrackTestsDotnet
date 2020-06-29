using System;
using OpenQA.Selenium;
using YouTrackWebdriverTests.Exceptions;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators
{
    public static class WebElementValidators
    {
        /// <exception cref="WrongTagException">element is not of a tagName tag.</exception>
        public static Action OfATag(string tagName, IWebElement element) =>
            () =>
            {
                if (element.TagName != tagName)
                {
                    throw new WrongTagException(tagName, element.TagName);
                }
            };
    }
}
