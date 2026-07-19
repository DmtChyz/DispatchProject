using DispatchProject.Enumerable.ActionLog;

namespace LogMicroservice.Entities
{
    public class ServiceLog
    {
        public ServiceLog()
        {
            CreatedAt = DateTime.UtcNow;
            Status = ActionStatus.Failed;
            ActionType = ActionType.Undefined;
        }

        public Guid Id { get; set; }
        public Guid? OwnerId { get; set; }
        public ActionType ActionType { get; set; }
        public string? Recipient { get; set; }
        public string? Details { get; set; }
        public string? CorrelationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ActionStatus Status { get; set; }
    }
}
