using Serilog;

namespace JoyCase.Api.Log
{
    public class LogService
    {
        public void LogInfo(string message)
        {
            Serilog.Log.Information(message);
        }

        public void LogWarning(string message)
        {
            Serilog.Log.Warning(message);
        }

        public void LogError(string message, Exception ex)
        {
            Serilog.Log.Error(ex, message);
        }
    }
}
