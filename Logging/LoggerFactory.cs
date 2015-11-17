namespace Affecto.Logging
{
    public abstract class LoggerFactory : ILoggerFactory
    {
        protected abstract ILogWriter GetLogWriter(object source);

        public ILogger CreateLogger(object source)
        {
            return new Logger(GetLogWriter(source));
        }

        public ICorrelationLogger CreateCorrelationLogger(object source)
        {
            return new Logger(GetLogWriter(source));
        }
    }
}