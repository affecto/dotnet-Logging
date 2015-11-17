using System;

namespace Affecto.Logging
{
    public interface ILogWriter
    {
        void WriteLog(LogEventLevel eventLevel, Exception exception, string formatMessage, params object[] args);
        void WriteLog(ICorrelation correlation, LogEventLevel eventLevel, Exception exception, string formatMessage, params object[] args);
    }
}