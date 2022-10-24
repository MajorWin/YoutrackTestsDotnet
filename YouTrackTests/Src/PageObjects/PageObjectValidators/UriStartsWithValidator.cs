using OpenQA.Selenium;
using YouTrackWebdriverTests.Exceptions;
using YouTrackWebdriverTests.SeleniumUtilities.Extensions;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators;

public class UriStartsWithValidator : IValidatable
{
    private readonly IWebDriver _browser;
    private readonly string _pathPart;
    private readonly int _timeoutSeconds;

    public UriStartsWithValidator(IWebDriver browser, string pathPart, int timeoutSeconds = 5)
    {
        _browser = browser;
        _pathPart = pathPart;
        _timeoutSeconds = timeoutSeconds;
    }

    public void Validate()
    {
        var (isUriValid, actualUri) = WaitHelpers.WaitForResult(
            () =>
            {
                var uri = _browser.GetUri();
                return (uri.AbsolutePath.StartsWith(_pathPart), uri);
            },
            _timeoutSeconds);
        if (!isUriValid)
        {
            throw new WrongUrlException(
                $"{actualUri.Authority}{_pathPart}...",
                $"{actualUri.Authority}{actualUri.AbsolutePath}"
            );
        }
    }
}