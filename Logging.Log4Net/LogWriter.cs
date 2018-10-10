using System;
using System.Reflection;
using log4net;
using log4net.Core;

namespace Affecto.Logging.Log4Net
{
    internal class LogWriter : ILogWriter
    {
        private readonly ILoggerRepository loggerRepository;
        private readonly Type sourceType;
        private ILog log;

        public LogWriter(ILoggerRepository loggerRepository, object source)
        {
            if (loggerRepository == null)
            {
                throw new ArgumentNullException("loggerRepository");
            }

            this.loggerRepository = loggerRepository;
            
            if (source != null)
            {
                sourceType = source.GetType();
            }
            else
            {
                sourceType = MethodBase.GetCurrentMethod().DeclaringType;
            }
        }

        public void WriteLog(ICorrelation correlation, LogEventLevel eventLevel, Exception exception, string formatMessage, params object[] args)
        {
            if (log == null)
            {
                log = loggerRepository.GetLogger(sourceType);
            }

            if (eventLevel == LogEventLevel.Verbose && !log.IsDebugEnabled)
            {
                return;
            }

            if (args != null && args.Length != 0)
            {
                formatMessage = string.Format(formatMessage, args);
            }

            log4net.Core.ILogger logger = log.Logger;

            LoggingEvent logEvent = new LoggingEvent(sourceType, logger.Repository, logger.Name, MapEventLevel(eventLevel), formatMessage, exception);

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

        private static Level MapEventLevel(LogEventLevel eventLevel)
        {
            switch (eventLevel)
            {
                case LogEventLevel.Verbose:
                    return Level.Verbose;
                case LogEventLevel.Debug:
                    return Level.Debug;
                case LogEventLevel.Information:
                    return Level.Info;
                case LogEventLevel.Warning:
                    return Level.Warn;
                case LogEventLevel.Error:
                    return Level.Error;
                case LogEventLevel.Critical:
                    return Level.Critical;
                default:
                    return Level.Info;
            }
        }
    }
}