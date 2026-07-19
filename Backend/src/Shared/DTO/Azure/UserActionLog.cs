using DispatchProject.Enumerable.ActionLog;

namespace Shared.DTO.Azure
{
    public class UserActionLog
    {
        public ActionType ActionType { get; set; }
        public string Recipient { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt {  get; set; }
        public ActionStatus Status { get; set; }
    }
}