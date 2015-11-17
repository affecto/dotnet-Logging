namespace Affecto.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(object source);
        ICorrelationLogger CreateCorrelationLogger(object source);
    }
}