using OpenQA.Selenium;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators;

public class WebElementExistsAndDisplayedNowValidator : IValidatable
{
    private readonly ISearchContext _searchContext;
    private readonly By _by;

    public WebElementExistsAndDisplayedNowValidator(ISearchContext searchContext, By by)
    {
        _searchContext = searchContext;
        _by = by;
    }

    /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
    /// <exception cref="ElementNotVisibleException">If searchContext's Displayed property is false.</exception>
    public void Validate()
    {
        var element = _searchContext.FindElement(_by);
        if (!element.Displayed)
        {
            throw new ElementNotVisibleException($"{_by} is not visible");
        }
    }
}