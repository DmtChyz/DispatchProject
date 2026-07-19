using DispatchProject.Enumerable.ActionLog;
namespace Shared.DTO.Azure
{
    public class LogMessage
    {
        public LogMessage()
        {
            ActionType = ActionType.Undefined;
            Status = ActionStatus.Failed;
            CreatedAt = DateTime.UtcNow;
        }
        public ActionType ActionType { get; set; }
        public string? Recipient { get; set; }
        public string? Details { get; set; }
        public string? CorrelationId { get; set; }
        public string? OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ActionStatus Status { get; set; }
    }
}
