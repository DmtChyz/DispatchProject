using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Azure
{
    public class DlqLogMessage
    {
        public DlqLogMessage()
        {
            DeadLetteredAt = DateTime.UtcNow;
        }
        public string? CorrelationId { get; set; }
        public string? QueueName { get; set; }
        public string? MessageBody { get; set; }
        public string? Reason { get; set; }
        public string? ErrorDescription { get; set; }
        public DateTime EnqueuedAt { get; set; }
        public DateTime DeadLetteredAt { get; set; }
    }
}
