using System;

namespace Affecto.Logging
{
    internal class Logger : ILogger, ICorrelationLogger
    {
        private readonly ILogWriter logWriter;

        public Logger(ILogWriter logWriter)
        {
            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }

            this.logWriter = logWriter;
        }

        #region Verbose

        public void LogVerbose(string formatMessage, params object[] args)
        {
            LogVerbose(null, formatMessage, args);
        }

        public void LogVerbose(ICorrelation correlation, string formatMessage, params object[] args)
        {
            logWriter.WriteLog(correlation, LogEventLevel.Verbose, null, formatMessage, args);
        }

        #endregion

        #region Information

        public void LogInformation(string formatMessage, params object[] args)
        {
            LogInformation(null, formatMessage, args);
        }

        public void LogInformation(ICorrelation correlation, string formatMessage, params object[] args)
        {
            logWriter.WriteLog(correlation, LogEventLevel.Information, null, formatMessage, args);
        }

        #endregion

        #region Warning

        public void LogWarning(string formatMessage, params object[] args)
        {
            LogWarning(null, null, formatMessage, args);
        }

        public void LogWarning(ICorrelation correlation, string formatMessage, params object[] args)
        {
            LogWarning(correlation, null, formatMessage, args);
        }

        public void LogWarning(Exception exception, string formatMessage, params object[] args)
        {
            LogWarning(null, exception, formatMessage, args);
        }

        public void LogWarning(ICorrelation correlation, Exception exception, string formatMessage, params object[] args)
        {
            logWriter.WriteLog(correlation, LogEventLevel.Warning, exception, formatMessage, args);
        }

        #endregion

        #region Error

        public void LogError(string formatMessage, params object[] args)
        {
            LogError(null, null, formatMessage, args);
        }

        public void LogError(ICorrelation correlation, string formatMessage, params object[] args)
        {
            LogError(correlation, null, formatMessage, args);
        }

        public void LogError(Exception exception, string formatMessage, params object[] args)
        {
            LogError(null, exception, formatMessage, args);
        }

        public void LogError(ICorrelation correlation, Exception exception, string formatMessage, params object[] args)
        {
            logWriter.WriteLog(correlation, LogEventLevel.Error, exception, formatMessage, args);
        }

        #endregion

        #region Critical

        public void LogCritical(string formatMessage, params object[] args)
        {
            LogCritical(null, null, formatMessage, args);
        }
        
        public void LogCritical(ICorrelation correlation, string formatMessage, params object[] args)
        {
            LogCritical(correlation, null, formatMessage, args);
        }

        public void LogCritical(Exception exception, string formatMessage, params object[] args)
        {
            LogCritical(null, exception, formatMessage, args);
        }

        public void LogCritical(ICorrelation correlation, Exception exception, string formatMessage, params object[] args)
        {
            logWriter.WriteLog(correlation, LogEventLevel.Critical, exception, formatMessage, args);
        }

        #endregion
    }
}