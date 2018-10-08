using System;
using Serilog;

namespace Affecto.Logging.Serilog
{
    public class SerilogLoggerFactory : LoggerFactory
    {
        private readonly int stackFramesToSkipForCallingTypeAndMethod;
        private static global::Serilog.ILogger loggerSingleton;
        private readonly object createLock = new object();

        public SerilogLoggerFactory(int stackFramesToSkipForCallingTypeAndMethod = 0)
        {
            if (stackFramesToSkipForCallingTypeAndMethod < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stackFramesToSkipForCallingTypeAndMethod));
            }

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
            return new LoggerConfiguration()
                .Enrich.WithCallingTypeAndMethod(stackFramesToSkipForCallingTypeAndMethod);
        }
    }
}