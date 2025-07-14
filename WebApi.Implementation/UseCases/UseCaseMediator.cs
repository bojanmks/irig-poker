using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IApplicationUser _applicationUser;
        private readonly IUseCaseLogger _useCaseLogger;
        private readonly IUseCaseSubscriberResolver _subscriberResolver;
        private readonly IValidatorResolver _validatorResolver;
        private readonly IUseCaseHandlerResolver _useCaseHandlerResolver;

        public UseCaseMediator(
            IMapper mapper,
            IApplicationUser applicationUser,
            IUseCaseLogger useCaseLogger,
            IUseCaseSubscriberResolver subscriberResolver,
            IValidatorResolver validatorResolver,
            IUseCaseHandlerResolver useCaseHandlerResolver
        )
        {
            _mapper = mapper;
            _applicationUser = applicationUser;
            _useCaseLogger = useCaseLogger;
            _subscriberResolver = subscriberResolver;
            _validatorResolver = validatorResolver;
            _useCaseHandlerResolver = useCaseHandlerResolver;
        }

        public Task<Result<TOut>> Execute<TUseCase, TData, TOut>(TUseCase useCase)
            where TUseCase : UseCase<TData, TOut>
        {
            var handler = _useCaseHandlerResolver.Resolve<TUseCase, TData, TOut>();

            if (handler is null)
            {
                throw new HandlerNotFoundException();
            }

            var executor = ConstructExecutor<TUseCase, TData, TOut>();

            return executor.Execute(useCase, handler);
        }

        private UseCaseExecutor<TUseCase, TData, TOut> ConstructExecutor<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>
        {
            return new UseCaseExecutor<TUseCase, TData, TOut>(_applicationUser, _useCaseLogger, _subscriberResolver, _validatorResolver);
        }
    }
}
