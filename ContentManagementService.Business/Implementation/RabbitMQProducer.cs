﻿using ContentManagementService.Business.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ContentManagementService.Business.Implementation
{
    public class RabbitMQProducer : IRabbitMQProducer, IDisposable
    {
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMQProducer()
        {
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            _connection = factory.CreateConnection();

            //Here we create channel with session and model
            _channel = _connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that
            _channel.QueueDeclare("notification_tasks", exclusive: false);
        }

        public void SendNotificationMessage<T>(T message)
        {
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            //put the data on to the product queue
            _channel.BasicPublish(exchange: string.Empty, routingKey: "notification_tasks", body: body);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_channel != null)
                {
                    _channel.Dispose();
                    _channel = null;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }
}