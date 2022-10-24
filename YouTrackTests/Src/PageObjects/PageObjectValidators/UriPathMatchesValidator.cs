using OpenQA.Selenium;
using YouTrackWebdriverTests.Exceptions;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators;

public class UriPathMatchesValidator : IValidatable
{
    private readonly IWebDriver _browser;
    private readonly string _path;
    private readonly int _timeoutSeconds;

    public UriPathMatchesValidator(IWebDriver browser, string path, int timeoutSeconds = 5)
    {
        _browser = browser;
        _path = path;
        _timeoutSeconds = timeoutSeconds;
    }

    public void Validate()
    {
        var (isUriValid, actualUri) = WaitHelpers.WaitForResult(
            () =>
            {
                var uri = _browser.GetUri();
                return (uri.AbsolutePath == _path, uri);
            },
            _timeoutSeconds);
        if (!isUriValid)
        {
            throw new WrongUrlException(
                $"{actualUri.Authority}{_path}",
                $"{actualUri.Authority}{actualUri.AbsolutePath}");
        } 
    }
}