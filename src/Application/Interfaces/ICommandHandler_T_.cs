using MediatR;

namespace Application.Interfaces
{
    internal interface ICommandHandler<in T> : IRequestHandler<T> where T : ICommand { }
}
