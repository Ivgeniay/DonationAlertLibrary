using DAlertsApi.Logger;

namespace ConsoleClient
{ 
    public class Logger : ILogger
    {
        private Action<string, LogLevel> action; 

        public void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            action?.Invoke(message, logLevel);
        }

        public ILogger LogAction(Action<string, LogLevel> action)
        {
            this.action = action;
            return this;
        }
    }
    
}
