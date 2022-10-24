using OpenQA.Selenium;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators;

public class WebElementExistsAndDisplayedValidator : IValidatable
{
    private readonly ISearchContext _searchContext;
    private readonly By _by;
    private readonly int _timeoutSeconds;

    public WebElementExistsAndDisplayedValidator(ISearchContext searchContext, By by, int timeoutSeconds = 5)
    {
        _searchContext = searchContext;
        _by = by;
        _timeoutSeconds = timeoutSeconds;
    }

    /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
    /// <exception cref="ElementNotVisibleException">If searchContext's Displayed property is false.</exception>
    public void Validate()
    {
        var displayed = WaitHelpers.Wait(
            () =>
            {
                try
                {
                    return _searchContext.FindElement(_by).Displayed;
                }
                catch (StaleElementReferenceException) { }
                catch (NoSuchElementException) { }

                return false;
            },
            _timeoutSeconds);

        // throw suppressed NoSuchElementException
        _searchContext.FindElement(_by);

        if (!displayed)
        {
            throw new ElementNotVisibleException($"{_by} is not visible");
        }
    }
}