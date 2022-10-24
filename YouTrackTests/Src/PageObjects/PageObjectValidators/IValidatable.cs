using System;

namespace YouTrackWebdriverTests.PageObjects.PageObjectValidators;

public interface IValidatable
{
    /// <exception cref="Exception">Throws exception if validation is failed</exception>
    void Validate();
}