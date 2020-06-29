using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace YouTrackApp
{
    public sealed class YouTrackRunner
    {
        private readonly string myJarPath;
        private readonly string myBaseUrl;
        private readonly string myPort;
        private readonly string myHomeDirectory;

        private readonly string myProgram;
        private readonly string myStartArguments;
        private readonly string myStopArguments;


        public YouTrackRunner(string jarPath, string baseUrl, string port, string homeDirectory)
        {
            myJarPath = jarPath;
            myBaseUrl = baseUrl;
            myPort = port;
            myHomeDirectory = Path.GetFullPath(homeDirectory);

            myProgram = "java";
            var args =
                $"-Djetbrains.youtrack.baseUrl={myBaseUrl}:{myPort} -Duser.home={myHomeDirectory} -jar {myJarPath}";
            myStartArguments = $"{args} {myPort}";
            myStopArguments = $"{args} stop {myPort}";
        }

        public void Start()
        {
            if (!File.Exists(myJarPath))
            {
                Console.WriteLine($"Can not run ${myJarPath}: file does not exist");
                return;
            }

            Console.WriteLine($"{myProgram} {myStartArguments}");

            Directory.CreateDirectory(myHomeDirectory);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = myProgram,
                    Arguments = myStartArguments,
                    UseShellExecute = true
                }
            };
            process.Start();

            Thread.Sleep(5000);
            Console.WriteLine($"YouTrack process: {process.Id}");
            Console.WriteLine($"YouTrack home directory: {myHomeDirectory}");
        }

        public void Stop()
        {
            if (!File.Exists(myJarPath))
            {
                Console.WriteLine($"Can not run ${myJarPath}: file does not exist");
                return;
            }

            Console.WriteLine($"{myProgram} {myStopArguments}");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = myProgram,
                    Arguments = myStopArguments,
                }
            };

            process.Start();
            process.WaitForExit();

            var isYoutrackStopped = process.WaitForExit(5000);

            // Wait for youtrack process to stop
            Thread.Sleep(2000);
        }

        public void CleanUp()
        {
            if (!File.Exists(myJarPath))
            {
                return;
            }

            Console.WriteLine($"Removing {myHomeDirectory}");
            Directory.Delete(myHomeDirectory, true);
        }
    }
}
