using System;
using Serilog;

namespace Affecto.Logging.Serilog
{
    public class SerilogLoggerFactory : LoggerFactory
    {
        private static global::Serilog.ILogger loggerSingleton;
        private readonly bool logCallingTypeAndMethod;
        private readonly int stackFramesToSkipForCallingTypeAndMethod;
        private readonly object createLock = new object();

        public SerilogLoggerFactory(bool logCallingTypeAndMethod = false, int stackFramesToSkipForCallingTypeAndMethod = 0)
        {
            if (stackFramesToSkipForCallingTypeAndMethod < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stackFramesToSkipForCallingTypeAndMethod));
            }

            this.logCallingTypeAndMethod = logCallingTypeAndMethod;
            this.stackFramesToSkipForCallingTypeAndMethod = stackFramesToSkipForCallingTypeAndMethod;
        }

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
            var configuration = new LoggerConfiguration();

            if (logCallingTypeAndMethod)
            {
                configuration.Enrich.WithCallingTypeAndMethod(stackFramesToSkipForCallingTypeAndMethod);
            }

            return configuration;
        }
    }
}