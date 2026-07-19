using DispatchProject.Enumerable.ActionLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Azure
{
    public class LogResponse
    {
        public string CorrelationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ActionStatus Status { get; set; } 
        public ActionType ActionType { get; set; }
        public string OwnerId { get; set; }
    }
}
