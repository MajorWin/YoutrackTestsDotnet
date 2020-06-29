using System;

namespace YouTrackWebdriverTests.Exceptions
{
    public class WrongUrlException : Exception
    {
        public WrongUrlException(string expected, string actual) : base(
            $"Expected current page's url to be {expected}, got {actual}") { }
    }
}