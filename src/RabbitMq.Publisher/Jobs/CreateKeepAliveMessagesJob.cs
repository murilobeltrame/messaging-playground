using Application.Messages.Command;
using MediatR;
using Quartz;

namespace RabbitMq.Publisher.Jobs
{
    internal class CreateKeepAliveMessagesJob : IJob
    {
        private readonly IMediator _mediator;

        public CreateKeepAliveMessagesJob(IMediator mediator) => _mediator = mediator;

        public Task Execute(IJobExecutionContext context) => _mediator.Send(new CreateMessageCommand("Keep Alive"));
    }
}
