namespace Affecto.Logging.NLog
{
    public class NLogLoggerFactory : LoggerFactory
    {
        private static readonly LoggerRepository RepositorySingleton = new LoggerRepository(new NLogWrapper());

        protected override ILogWriter GetLogWriter(object source)
        {
            return new LogWriter(RepositorySingleton, source);
        }
    }
}
