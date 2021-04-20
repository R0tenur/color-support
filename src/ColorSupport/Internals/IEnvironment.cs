using System;

namespace ColorSupport.Internals
{
    public interface IEnvironment
    {
        bool HasFlag(string flag);
        string GetEnvironmentVariable(string variable);
        bool HasEnvironmentVariable(string variable);
        bool IsWindows();
        Version GetOsVersion();
        bool ConsoleIsRedirected();
    }
}