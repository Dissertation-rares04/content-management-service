using ContentManagementService.Core.Enum;

namespace ContentManagementService.Core.Model
{
    public class Interaction
    {
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public InteractionType InteractionType { get; set; }
    }
}
