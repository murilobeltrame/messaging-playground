using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Infra
{
    public class RabbitMqBroker : BaseBroker
    {
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMqBroker> _logger;

        public RabbitMqBroker(
            ILogger<RabbitMqBroker> logger,
            IMediator mediator,
            BrokerOptions options) : base(mediator)
        {
            var factory = new ConnectionFactory { HostName = options.ConnectionString };
            _connection = factory.CreateConnection();
            _logger = logger;
        }

        public override Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        {
            using (var channel = _connection.CreateModel())
            {
                var isTopic = !string.IsNullOrWhiteSpace(@event.TopicName);
                channel.ExchangeDeclare(@event.ExchangeName, isTopic ? ExchangeType.Topic : ExchangeType.Fanout, true, false);

                var props = channel.CreateBasicProperties();
                props.MessageId = Guid.NewGuid().ToString();
                props.ContentType = @event.ExchangeName;
                props.DeliveryMode = 2;
                var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                channel.BasicPublish(@event.ExchangeName, @event.TopicName ?? string.Empty, props, message);
            }
            return Task.CompletedTask;
        }

        public override Task ReceiveAsync<TEvent, TCommand>(string exchangeName, string topicName, string queueName, Action<TEvent> handler, CancellationToken cancellationToken)
        {
            var channel = _connection.CreateModel();

            var args = CreateDeadLetterPolicy(channel, exchangeName);
            channel.QueueDeclare(queueName, true, false, false, args);
            channel.BasicQos(0, 10, false);
            if (!string.IsNullOrWhiteSpace(exchangeName))
            {
                var isTopic = !string.IsNullOrWhiteSpace(topicName);
                channel.ExchangeDeclare(exchangeName, isTopic ? ExchangeType.Topic : ExchangeType.Fanout, true, false, null);
                channel.QueueBind(queueName, exchangeName, isTopic ? topicName : queueName);
            }
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => {
                try
                {
                    var messageBody = ea.Body.ToArray();
                    var messageJson = Encoding.UTF8.GetString(messageBody);
                    var message = JsonConvert.DeserializeObject<TEvent>(messageJson);
                    if (message != null)
                    {
                        handler(message);
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar recedimento da mensagem");
                    throw;
                }
            };

            channel.BasicConsume(queueName, false, consumer);
            return Task.CompletedTask;
        }

        private Dictionary<string, object> CreateDeadLetterPolicy(IModel channel, string exchangeName)
        {
            var dlxExhange = $"{exchangeName}-deadletter";
            var dlxQueue = "Messages-DLX";
            channel.ExchangeDeclare(dlxExhange, ExchangeType.Fanout, true, false, null);
            channel.QueueDeclare(dlxQueue, true, false, false, null);
            channel.QueueBind(dlxQueue, dlxExhange, string.Empty, null);
            var args = new Dictionary<string, object> { { "x-dead-letter-exchange", dlxExhange }, { "x-max-priority", 10 } };
            return args;
        }
    }
}
