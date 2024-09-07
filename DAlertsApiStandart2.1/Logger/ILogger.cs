using System;

namespace DAlertsApi.Logger
{
    public interface ILogger
    {
        public void Log(string message, LogLevel logLevel = LogLevel.Info);
        public ILogger LogAction(Action<string, LogLevel> action);
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

}
