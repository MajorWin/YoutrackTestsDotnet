using System;

namespace YouTrackWebdriverTests.Exceptions
{
    public class WrongTagException : Exception
    {
        public WrongTagException(string expected, string actual) : base(
            $"Wrong tag {actual}, expected to get {expected}") { }
    }
}