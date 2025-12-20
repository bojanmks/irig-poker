using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Core.UseCases;

namespace WebApi.Implementation.Core.UseCases.Resolvers
{
    public class ServiceProviderUseCaseSubscriberResolver : IUseCaseSubscriberResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderUseCaseSubscriberResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IUseCaseSubscriber<TUseCase, TData, TOut>> ResolveAll<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            var subscribers = _serviceProvider.GetService<IEnumerable<IUseCaseSubscriber<TUseCase, TData, TOut>>>();

            if (subscribers is null)
            {
                return Enumerable.Empty<IUseCaseSubscriber<TUseCase, TData, TOut>>();
            }

            return subscribers;
        }
    }
}
