using WebApi.Application.ApplicationUsers;
using WebApi.Application.Logging;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Common.DTO.Result;
using WebApi.Implementation.Exceptions;

namespace WebApi.Implementation.UseCases
{
    public class UseCaseMediator
    {
        private readonly IApplicationUserResolver _applicationUserResolver;
        private readonly IUseCaseLogger _useCaseLogger;
        private readonly IUseCaseSubscriberResolver _subscriberResolver;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IUseCaseHandlerResolver _useCaseHandlerResolver;

        public UseCaseMediator(
            IApplicationUserResolver applicationUserResolver,
            IUseCaseLogger useCaseLogger,
            IUseCaseSubscriberResolver subscriberResolver,
            IValidatorResolver validatorResolver,
            IUseCaseHandlerResolver useCaseHandlerResolver
        )
        {
            _applicationUserResolver = applicationUserResolver;
            _useCaseLogger = useCaseLogger;
            _subscriberResolver = subscriberResolver;
            _validatorResolver = validatorResolver;
            _useCaseHandlerResolver = useCaseHandlerResolver;
        }

        public Task<Result<TOut>> Execute<TUseCase, TData, TOut>(TUseCase useCase, CancellationToken cancellationToken = default)
            where TUseCase : UseCase<TData, TOut>
        {
            var handler = _useCaseHandlerResolver.Resolve<TUseCase, TData, TOut>();

            if (handler is null)
            {
                throw new HandlerNotFoundException();
            }

            var executor = ConstructExecutor<TUseCase, TData, TOut>();

            return executor.ExecuteAsync(useCase, handler, cancellationToken);
        }

        private UseCaseExecutor<TUseCase, TData, TOut> ConstructExecutor<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            return new UseCaseExecutor<TUseCase, TData, TOut>(_applicationUserResolver, _useCaseLogger, _subscriberResolver, _validatorResolver);
        }
    }
}
