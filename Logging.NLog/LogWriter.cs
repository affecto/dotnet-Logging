using System;
using System.Globalization;
using System.Reflection;
using nLog = NLog;

namespace Affecto.Logging.NLog
{
    internal class LogWriter : ILogWriter
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly Type _sourceType;
        private nLog.ILogger _logger;

        public LogWriter(ILoggerRepository loggerRepository, object source)
        {
            if (loggerRepository == null)
            {
                throw new ArgumentNullException(nameof(loggerRepository));
            }

            this._loggerRepository = loggerRepository;

            _sourceType = source?.GetType() ?? MethodBase.GetCurrentMethod().DeclaringType;
        }

        public void WriteLog(ICorrelation correlation, LogEventLevel eventLevel, Exception exception, string message, params object[] args)
        {
            if (_logger == null)
            {
                _logger = _loggerRepository.GetLogger(_sourceType);
            }

            if (eventLevel == LogEventLevel.Verbose && !_logger.IsDebugEnabled)
            {
                return;
            }

            var logEvent = new nLog.LogEventInfo(MapEventLevel(eventLevel), _logger.Name, CultureInfo.CurrentCulture, message, args, exception);

            if (correlation != null)
            {
                logEvent.Properties["CallerId"] = correlation.CallerId;
                logEvent.Properties["CorrelationId"] = correlation.CorrelationId;
            }

            _logger.Log(logEvent);
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
