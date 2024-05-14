using Confluent.Kafka;

namespace ContentManagementService.Core.AppSettings
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }


        public string ClientId { get; set; }


        public SecurityProtocol SecurityProtocol { get; set; }


        public SaslMechanism SaslMechanism { get; set; }


        public string SaslUsername { get; set; }


        public string SaslPassword { get; set; }
    }
}
