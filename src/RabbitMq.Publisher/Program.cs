// See https://aka.ms/new-console-template for more information
using Application.Messages;
using Infra;
using Infra.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using RabbitMq.Publisher.Jobs;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) => {
        var brokerConnectionString = context.Configuration.GetValue<string>("rabbitmq");
        services.AddBroker<RabbitMqBroker>(o => o.ConnectionString = brokerConnectionString!);
        services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<MessagesHandler>());

        services.AddQuartz(o =>
        {
            o.UseMicrosoftDependencyInjectionJobFactory();
            o.ScheduleJob<CreateKeepAliveMessagesJob>(x => x
                .StartNow()
                .WithSimpleSchedule(y => y
                    .WithIntervalInSeconds(15)
                    .RepeatForever()));
        });
        services.AddQuartzHostedService(o => o.WaitForJobsToComplete = true);
    });

var app = builder.Build();

await app.RunAsync();