namespace ContentManagementService.Business.Interface
{
    public interface IRabbitMQProducer
    {
        public void SendNotificationMessage<T>(T message);
    }
}
