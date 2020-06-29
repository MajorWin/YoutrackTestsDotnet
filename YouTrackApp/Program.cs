using System;

namespace YouTrackApp
{
    static class Program
    {
        private class StartArgs
        {
            public string YoutrackJarPath { get; set; }
            public string BaseUrl { get; set; }
            public string Port { get; set; }
            public string HomeDirectory { get; set; }
            public bool StopCleanup { get; set; }
            public bool Help { get; set; }
        }

        private const string DefaultJarPath = @".\Resources\youtrack-5.2.5-8823.jar";
        private const string DefaultBaseUrl = "http://localhost";
        private const string DefaultPort = "8080";
        private const string DefaultHomeDirectory = @".\YoutrackData";

        private static readonly string HelpString =
            @$"Arguments:
/YoutrackJarPath <path> ({DefaultJarPath})
/BaseUrl <url> ({DefaultBaseUrl})
/Port <port> ({DefaultPort})
/HomeDirectory <dir> ({DefaultHomeDirectory})
/StopCleanup
/Help";


        private static void Main(string[] args)
        {
            var arguments = Args.Configuration.Configure<StartArgs>().CreateAndBind(args);
            if (arguments.Help)
            {
                Console.WriteLine(HelpString);
                return;
            }

            arguments.YoutrackJarPath ??= DefaultJarPath;
            arguments.BaseUrl ??= DefaultBaseUrl;
            arguments.Port ??= DefaultPort;
            arguments.HomeDirectory ??= DefaultHomeDirectory;

            var youtrack = new YouTrackRunner(
                arguments.YoutrackJarPath,
                arguments.BaseUrl,
                arguments.Port,
                arguments.HomeDirectory);

            if (arguments.StopCleanup)
            {
                youtrack.Stop();
                youtrack.CleanUp();
                return;
            }

            youtrack.Start();
        }
    }
}
