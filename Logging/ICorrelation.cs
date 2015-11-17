namespace Affecto.Logging
{
    public interface ICorrelation
    {
        string CorrelationId { get; }
        string CallerId { get; }
    }
}