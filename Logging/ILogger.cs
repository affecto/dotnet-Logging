using System;

namespace Affecto.Logging
{
    public interface ILogger
    {
        void Log(LogEventLevel level, string formatMessage, params object[] args);
        void LogVerbose(string formatMessage, params object[] args);
        void LogDebug(string formatMessage, params object[] args);
        void LogInformation(string formatMessage, params object[] args);
        void LogWarning(string formatMessage, params object[] args);
        void LogWarning(Exception exception, string formatMessage, params object[] args);
        void LogError(string formatMessage, params object[] args);
        void LogError(Exception exception, string formatMessage, params object[] args);
        void LogCritical(string formatMessage, params object[] args);
        void LogCritical(Exception exception, string formatMessage, params object[] args);
    }
}