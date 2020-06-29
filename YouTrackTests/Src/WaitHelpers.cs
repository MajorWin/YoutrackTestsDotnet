using System;
using System.Diagnostics;
using System.Threading;
using YouTrackWebdriverTests.Extensions;

namespace YouTrackWebdriverTests
{
    public static class WaitHelpers
    {
        // https://habr.com/ru/post/443754/
        public static bool Wait(Func<bool> condition, int timeoutSeconds = 5, int checkIntervalMillis = 100)
        {
            var timeout = timeoutSeconds.Seconds();

            var sw = Stopwatch.StartNew();
            while (sw.Elapsed < timeout)
            {
                if (condition())
                {
                    return true;
                }

                Thread.Sleep(checkIntervalMillis);
            }

            return false;
        }

        public static (bool success, T result) WaitForResult<T>(
            Func<(bool success, T result)> getResult,
            int timeoutSeconds = 5,
            int checkIntervalMillis = 100)
        {
            var timeout = timeoutSeconds.Seconds();

            var sw = Stopwatch.StartNew();
            while (sw.Elapsed < timeout)
            {
                var (success, result) = getResult();
                if (success)
                {
                    return (true, result);
                }

                Thread.Sleep(checkIntervalMillis);
            }

            return (false, default);
        }

        public static bool WaitIgnoringException<E>(
            Func<bool> condition,
            int timeoutSeconds = 5,
            int checkIntervalMillis = 100) where E : Exception
        {
            var timeout = timeoutSeconds.Seconds();

            var sw = Stopwatch.StartNew();
            while (sw.Elapsed < timeout)
            {
                try
                {
                    if (condition())
                    {
                        return true;
                    }
                }
                catch (E) { }

                Thread.Sleep(checkIntervalMillis);
            }

            return false;
        }
    }
}
