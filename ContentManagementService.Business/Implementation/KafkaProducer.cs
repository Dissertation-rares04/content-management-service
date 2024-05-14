using Confluent.Kafka;
using ContentManagementService.Business.Interface;
using ContentManagementService.Core.AppSettings;
using ContentManagementService.Core.Enum;
using ContentManagementService.Core.Model;
using ContentManagementService.Data.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ContentManagementService.Business.Implementation
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ProducerConfig _producerConfig;
        private readonly string _topic;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings, IOptions<KafkaTopics> kafkaTopics)
        {
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                ClientId = kafkaSettings.Value.ClientId,
                SecurityProtocol = SecurityProtocol.Plaintext
                //SecurityProtocol = kafkaSettings.Value.SecurityProtocol,
                //SaslMechanism = kafkaSettings.Value.SaslMechanism,
                //SaslUsername = kafkaSettings.Value.SaslUsername,
                //SaslPassword = kafkaSettings.Value.SaslPassword
            };
            _topic = kafkaTopics.Value.UserInteractions;
        }

        public async Task ProduceLikeEvent(Post post, Like like)
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

            var likeEvent = new
            {
                PostId = post.Id,
                UserId = like.UserId,
                Category = post.Category,
                Timestamp = ((DateTimeOffset)like.CreatedAt).ToUnixTimeSeconds(),
                Action = ActionType.POST_LIKED,
            };

            var message = JsonConvert.SerializeObject(likeEvent);

            var result = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message }); ;
        }
    }
}
