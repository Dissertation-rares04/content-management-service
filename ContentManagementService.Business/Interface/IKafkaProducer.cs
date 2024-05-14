using ContentManagementService.Core.Model;

namespace ContentManagementService.Business.Interface
{
    public interface IKafkaProducer
    {
        Task ProduceLikeEvent(Post post, Like like);
    }
}
