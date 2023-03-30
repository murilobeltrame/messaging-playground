using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBroker<TBroker>(this IServiceCollection services, Action<BrokerOptions> configure)
            where TBroker : class, IBroker
        {
            services.AddSingleton<IBroker, TBroker>();
            services.AddSingleton(BrokerOptions.FromAction(configure));
            return services;
        }
    }
}
