using ContentManagementService.Core.Model;

namespace ContentManagementService.Business.Interface
{
    public interface IKafkaProducer
    {
        Task ProduceInteractionEvent(Post post, Interaction interaction);
    }
}
