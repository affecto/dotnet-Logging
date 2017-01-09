using System;
using System.Globalization;
using System.Reflection;
using nLog = NLog;

namespace Affecto.Logging.NLog
{
    internal class LogWriter : ILogWriter
    {
        private readonly ILoggerRepository loggerRepository;
        private readonly Type sourceType;
        private nLog.ILogger logger;

        public LogWriter(ILoggerRepository loggerRepository, object source)
        {
            if (loggerRepository == null)
            {
                throw new ArgumentNullException(nameof(loggerRepository));
            }

            this.loggerRepository = loggerRepository;

            sourceType = source?.GetType() ?? MethodBase.GetCurrentMethod().DeclaringType;
        }

        public void WriteLog(ICorrelation correlation, LogEventLevel eventLevel, Exception exception, string message, params object[] args)
        {
            if (logger == null)
            {
                logger = loggerRepository.GetLogger(sourceType);
            }

            if (eventLevel == LogEventLevel.Verbose && !logger.IsDebugEnabled)
            {
                return;
            }

            var logEvent = new nLog.LogEventInfo(MapEventLevel(eventLevel), logger.Name, CultureInfo.CurrentCulture, message, args, exception);

            if (correlation != null)
            {
                logEvent.Properties["CallerId"] = correlation.CallerId;
                logEvent.Properties["CorrelationId"] = correlation.CorrelationId;
            }

            logger.Log(logEvent);
        }

        public void WriteLog(LogEventLevel eventLevel, Exception exception, string formatMessage, params object[] args)
        {
            WriteLog(null, eventLevel, exception, formatMessage, args);
        }

        private static nLog.LogLevel MapEventLevel(LogEventLevel eventLevel)
        {
            switch (eventLevel)
            {
                case LogEventLevel.Verbose:
                    return nLog.LogLevel.Debug;
                case LogEventLevel.Information:
                    return nLog.LogLevel.Info;
                case LogEventLevel.Warning:
                    return nLog.LogLevel.Warn;
                case LogEventLevel.Error:
                    return nLog.LogLevel.Error;
                case LogEventLevel.Critical:
                    return nLog.LogLevel.Fatal;
                default:
                    return nLog.LogLevel.Info;
            }
        }
    }
}
