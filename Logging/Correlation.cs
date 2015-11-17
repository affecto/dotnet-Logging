namespace Affecto.Logging
{
    public class Correlation : ICorrelation
    {
        public Correlation(string correlationId, string callerId = null)
        {
            CorrelationId = correlationId;
            CallerId = callerId;
        }

        public string CorrelationId { get; set; }
        public string CallerId { get; set; }
    }
}