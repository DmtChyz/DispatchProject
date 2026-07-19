using DispatchProject.Enumerable.ActionLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Azure
{
    public class NotificationMessage
    {
        public ActionType ActionType { get; set; }
        public string Recipient { get; set; }
        public string Details { get; set; }
        public string CorrelationId { get; set; }
        public string OwnerId { get; set; }
    }
}
