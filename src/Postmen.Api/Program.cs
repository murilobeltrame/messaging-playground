
using Application.Messages;
using Infra;
using Infra.Extensions;

namespace Postmen.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddBroker<RabbitMqBroker>(o => {
                var connString = builder.Configuration.GetValue<string>("rabbitmq");
                o.ConnectionString = connString!;
            });
            builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<MessagesHandler>());
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}