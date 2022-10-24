using OpenQA.Selenium;
using YouTrackWebdriverTests.Exceptions;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators;

public class TagValidator : IValidatable
{
    private readonly IWebElement _element;
    private readonly string _tagName;

    public TagValidator(IWebElement element, string tagName)
    {
        _element = element;
        _tagName = tagName;
    }

    public void Validate()
    {
        if (_element.TagName != _tagName)
        {
            throw new WrongTagException(_tagName, _element.TagName);
        }
    }
}