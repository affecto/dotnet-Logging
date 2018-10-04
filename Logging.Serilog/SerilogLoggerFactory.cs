using Serilog;

namespace Affecto.Logging.Serilog
{
    public class SerilogLoggerFactory : LoggerFactory
    {
        private static global::Serilog.ILogger loggerSingleton;
        private readonly object createLock = new object();

        protected override ILogWriter GetLogWriter(object source)
        {
            if (loggerSingleton == null)
            {
                lock (createLock)
                {
                    if (loggerSingleton == null)
                    {
                        loggerSingleton = CreateLoggerConfiguration().CreateLogger();
                    }
                }
            }

            return new LogWriter(loggerSingleton);
        }

        protected virtual LoggerConfiguration CreateLoggerConfiguration()
        {
            return new LoggerConfiguration()
                .Enrich.WithCaller();
        }
    }
}