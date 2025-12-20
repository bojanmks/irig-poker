using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Core.UseCases;

namespace WebApi.Implementation.Core.UseCases.Resolvers
{
    public class ServiceProviderUseCaseHandlerResolver : IUseCaseHandlerResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderUseCaseHandlerResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public UseCaseHandler<TUseCase, TData, TOut> Resolve<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            return _serviceProvider.GetService<UseCaseHandler<TUseCase, TData, TOut>>();
        }
    }
}
