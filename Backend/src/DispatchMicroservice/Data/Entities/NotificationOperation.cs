using Shared.Enumerable;

namespace DispatchMicroservice.Data.Entities
{
    public class NotificationOperation
    {
        public string CorrelationId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public NotificationOperationStatus Status { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }
    }
}