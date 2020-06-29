using System;

namespace YouTrackWebdriverTests.Extensions
{
    public static class IntExtensions
    {
        public static TimeSpan Seconds(this int seconds) => TimeSpan.FromSeconds(seconds);
        public static TimeSpan Millis(this int millis) => TimeSpan.FromMilliseconds(millis);
    }
}