// See https://aka.ms/new-console-template for more information
using Application.Interfaces;
using Application.Messages;
using Application.Messages.Command;
using Infra;
using Infra.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMq.Consumer.IntegrationEvents;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) => {
        var brokerConnectionString = context.Configuration.GetValue<string>("rabbitmq");
        services.AddBroker<RabbitMqBroker>(o => o.ConnectionString = brokerConnectionString!);
        services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<MessagesHandler>());
    });

var app = builder.Build();

var _broker = app.Services.GetRequiredService<IBroker>();
await _broker.ReceiveAsync<MessageCreatedEvent, ProcessMessageCommand>(nameof(MessageCreatedEvent), "consume_message_created"); // TODO: Como estabelecer o evento mas deixar o commando implicito

await app.RunAsync();
