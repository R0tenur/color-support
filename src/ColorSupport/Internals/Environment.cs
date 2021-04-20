using System;
using System.Runtime.InteropServices;

namespace ColorSupport.Internals
{
    internal class Environment : IEnvironment
    {
        private readonly string[] _args;
        internal Environment()
        {
            _args = System.Environment.GetCommandLineArgs();
        }
        internal Environment(string[] args)
        {
            _args = args;
        }
        public bool HasFlag(string flag)
        {
            var prefix = flag.StartsWith("-") ? string.Empty : (flag.Length == 1 ? "-" : "--");

            var position = Array.IndexOf(_args, prefix + flag);

            var terminatorPosition = Array.IndexOf(_args, "--");
            return position != -1 && (terminatorPosition == -1 || position < terminatorPosition);
        }
        public string GetEnvironmentVariable(string variable) => System.Environment.GetEnvironmentVariable(variable);
        public bool HasEnvironmentVariable(string variable) => !string.IsNullOrWhiteSpace(System.Environment.GetEnvironmentVariable(variable));
        public bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public Version GetOsVersion() => System.Environment.OSVersion.Version;
        public bool ConsoleIsRedirected() => Console.IsInputRedirected;
    }
}