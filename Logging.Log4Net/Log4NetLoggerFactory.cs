namespace Affecto.Logging.Log4Net
{
    public class Log4NetLoggerFactory : LoggerFactory
    {
        private static readonly LoggerRepository RepositorySingleton = new LoggerRepository(new Log4NetWrapper());

        protected override ILogWriter GetLogWriter(object source)
        {
            return new LogWriter(RepositorySingleton, source);
        }
    }
}