using Confluent.Kafka;
using ContentManagementService.Business.Interface;
using ContentManagementService.Core.AppSettings;
using ContentManagementService.Core.Model;
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

        public async Task ProduceInteractionEvent(Post post, Interaction interaction)
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

            var interactionEvent = new
            {
                PostId = post.Id,
                UserId = interaction.UserId,
                Category = post.Category,
                Timestamp = ((DateTimeOffset)interaction.CreatedAt).ToUnixTimeSeconds(),
                Action = interaction.InteractionType,
            };

            var message = JsonConvert.SerializeObject(interactionEvent);

            var result = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message }); ;
        }
    }
}
