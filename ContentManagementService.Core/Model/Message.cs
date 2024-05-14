using ContentManagementService.Core.Enum;

namespace ContentManagementService.Core.Model
{
    public class Message
    {
        public ActionType ActionType { get; set; }

        public string Value { get; set; }
    }
}
