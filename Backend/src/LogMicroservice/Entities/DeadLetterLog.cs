namespace LogMicroservice.Entities
{
    public class DeadLetterLog
    {
        public Guid Id { get; set; }
        public string? CorrelationId { get; set; }
        public string? QueueName { get; set; }
        public string? MessageBody { get; set; }
        public string? Reason { get; set; }
        public string? ErrorDescription { get; set; }
        public DateTime EnqueuedAt { get; set; }
        public DateTime DeadLetteredAt { get; set; }
    }
}
