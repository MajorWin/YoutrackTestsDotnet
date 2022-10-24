using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;

namespace YouTrackWebdriverTests.PageObjects.Base;

public abstract class ValidatedWebObject
{
    protected readonly IWebDriver Browser;

    protected ValidatedWebObject(IWebDriver browser, IValidatable validator)
    {
        Browser = browser;

        validator.Validate();
    }
}