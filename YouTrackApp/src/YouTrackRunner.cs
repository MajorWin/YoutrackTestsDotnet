using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace YouTrackApp
{
    public sealed class YouTrackRunner
    {
        private const string PidFileName = "youtrack.pid";
        private const string Program = "java";
        private const string ArgumentsTemplate = "-Djetbrains.youtrack.baseUrl={0}:{1} -Duser.home={2} -jar {3}";
        private const string StartArgumentsTemplate = ArgumentsTemplate + " {4}";
        private const string StopArgumentsTemplate = ArgumentsTemplate + " stop {4}";

        private readonly string _jarPath;
        private readonly string _baseUrl;
        private readonly string _port;
        private readonly string _homeDirectory;

        private readonly string _startArguments;
        private readonly string _stopArguments;

        private readonly string _pidPath;


        public YouTrackRunner(string jarPath, string baseUrl, string port, string homeDirectory)
        {
            _jarPath = jarPath;
            _baseUrl = baseUrl;
            _port = port;
            _homeDirectory = Path.GetFullPath(homeDirectory);

            _startArguments = string.Format(StartArgumentsTemplate, _baseUrl, _port, _homeDirectory, _jarPath, _port);
            _stopArguments = string.Format(StopArgumentsTemplate, _baseUrl, _port, _homeDirectory, _jarPath, _port);

            _pidPath = Path.Combine(_homeDirectory, PidFileName);
        }

        public void Start()
        {
            if (!File.Exists(_jarPath))
            {
                Console.WriteLine($"Can not run ${_jarPath}: file does not exist");
                return;
            }

            try
            {
                var pid = ReadYoutrackPid();

                var p = Process.GetProcessById(pid);
                if (p.HasExited)
                    throw new ArgumentException("https://github.com/dotnet/runtime/issues/66096");

                Console.WriteLine(
                    $"There is youtrack pid file at :'\"{_pidPath}\" and a process with the same pid ({pid}) running.");
                return;
            }
            catch (ArgumentException noSuchProcessException)
            {
            }

            Console.WriteLine($"{Program} {_startArguments}");

            Directory.CreateDirectory(_homeDirectory);

            var youtrack = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Program,
                    Arguments = _startArguments,
                    UseShellExecute = true
                }
            };
            youtrack.Start();

            Thread.Sleep(1000);
            Console.WriteLine($"YouTrack process: {youtrack.Id}");
            Console.WriteLine($"YouTrack home directory: {_homeDirectory}");

            WriteYoutrackPid(youtrack);
            Console.WriteLine($"Youtrack pid file: {_pidPath}");
        }

        public void Stop()
        {
            if (!File.Exists(_pidPath))
            {
                Console.WriteLine($"There is no youtrack pid file at: {_pidPath}");
                return;
            }

            var pid = ReadYoutrackPid();
            Process youtrack;
            try
            {
                youtrack = Process.GetProcessById(pid);
            }
            catch (ArgumentException noSuchProcessException)
            {
                Console.WriteLine($"No youtrack process running");
                throw;
            }

            if (!File.Exists(_jarPath))
            {
                Console.WriteLine($"Can not run ${_jarPath}: file does not exist");
                return;
            }

            Console.WriteLine($"{Program} {_stopArguments}");

            var youtrackStopper = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Program,
                    Arguments = _stopArguments,
                }
            };

            youtrackStopper.Start();
            youtrackStopper.WaitForExit(5000);

            if (!youtrack.WaitForExit(10000))
                youtrack.Kill();
            DeleteYoutrackPid();
        }

        public void CleanUp()
        {
            Console.WriteLine($"Removing {_homeDirectory}");

            if (!Directory.Exists(_homeDirectory))
                return;

            Directory.Delete(_homeDirectory, true);
        }

        private int ReadYoutrackPid()
        {
            if (!File.Exists(_pidPath))
                return -1;

            return int.TryParse(File.ReadAllText(_pidPath), out var pid) ? pid : -1;
        }

        private void WriteYoutrackPid(Process youtrack) => File.WriteAllText(_pidPath, youtrack.Id.ToString());

        private void DeleteYoutrackPid() => File.Delete(_pidPath);
    }
}