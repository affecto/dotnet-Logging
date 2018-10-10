using System;

namespace Affecto.Logging
{
    public interface ICorrelationLogger
    {
        void Log(ICorrelation correlation, LogEventLevel level, string formatMessage, params object[] args);
        void LogVerbose(ICorrelation correlation, string formatMessage, params object[] args);
        void LogDebug(ICorrelation correlation, string formatMessage, params object[] args);
        void LogInformation(ICorrelation correlation, string formatMessage, params object[] args);
        void LogWarning(ICorrelation correlation, string formatMessage, params object[] args);
        void LogWarning(ICorrelation correlation, Exception exception, string formatMessage, params object[] args);
        void LogError(ICorrelation correlation, string formatMessage, params object[] args);
        void LogError(ICorrelation correlation, Exception exception, string formatMessage, params object[] args);
        void LogCritical(ICorrelation correlation, string formatMessage, params object[] args);
        void LogCritical(ICorrelation correlation, Exception exception, string formatMessage, params object[] args);
    }
}