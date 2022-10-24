using System;
using OpenQA.Selenium;
using YouTrackWebdriverTests.PageObjects.CommonObjects;
using YouTrackWebdriverTests.PageObjects.PageObjectValidators;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.Base;

public abstract class YoutrackPageObject : ValidatedWebObject
{
    protected YoutrackPageObject(IWebDriver browser, IValidatable validator) : base(browser, validator)
    {
    }


    // [LogAspect]
    public ErrorPopup GetErrorPopup() => new ErrorPopup(Browser);

    public Uri GetUri() => Browser.GetUri();
}