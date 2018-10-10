using System;
using Serilog.Context;

namespace Affecto.Logging.Serilog
{
    internal class LogWriter : ILogWriter
    {
        private readonly global::Serilog.ILogger logger;

        public LogWriter(global::Serilog.ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void WriteLog(ICorrelation correlation, LogEventLevel eventLevel, Exception exception, string formatMessage, params object[] args)
        {
            using (LogContext.PushProperty(nameof(correlation.CallerId), correlation?.CallerId))
            using (LogContext.PushProperty(nameof(correlation.CorrelationId), correlation?.CorrelationId))
            {
                WriteLog(eventLevel, exception, formatMessage, args);
            }
        }

        public void WriteLog(LogEventLevel eventLevel, Exception exception, string formatMessage, params object[] args)
        {
            logger.Write(MapEventLevel(eventLevel), exception, formatMessage, args);
        }

        private static global::Serilog.Events.LogEventLevel MapEventLevel(LogEventLevel eventLevel)
        {
            switch (eventLevel)
            {
                case LogEventLevel.Verbose:
                    return global::Serilog.Events.LogEventLevel.Verbose;
                case LogEventLevel.Information:
                    return global::Serilog.Events.LogEventLevel.Information;
                case LogEventLevel.Warning:
                    return global::Serilog.Events.LogEventLevel.Warning;
                case LogEventLevel.Error:
                    return global::Serilog.Events.LogEventLevel.Error;
                case LogEventLevel.Critical:
                    return global::Serilog.Events.LogEventLevel.Fatal;
                default:
                    return global::Serilog.Events.LogEventLevel.Information;
            }
        }
    }
}