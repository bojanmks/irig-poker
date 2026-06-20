using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Logging;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Core.Validation;
using WebApi.Common.Core.Result.Models;
using WebApi.Implementation.Core.UseCases.Exceptions;

namespace WebApi.Implementation.Core.UseCases.Execution;

public class UseCaseMediator
{
    private readonly IApplicationUserResolver _applicationUserResolver;
    private readonly IUseCaseLogger _useCaseLogger;
    private readonly IValidatorResolver _validatorResolver;
    private readonly IUseCaseHandlerResolver _useCaseHandlerResolver;

    public UseCaseMediator(
        IApplicationUserResolver applicationUserResolver,
        IUseCaseLogger useCaseLogger,
        IValidatorResolver validatorResolver,
        IUseCaseHandlerResolver useCaseHandlerResolver
    )
    {
        _applicationUserResolver = applicationUserResolver;
        _useCaseLogger = useCaseLogger;
        _validatorResolver = validatorResolver;
        _useCaseHandlerResolver = useCaseHandlerResolver;
    }

    public Task<Result<TOut>> ExecuteAsync<TUseCase, TData, TOut>(TUseCase useCase, CancellationToken cancellationToken = default)
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
        return new UseCaseExecutor<TUseCase, TData, TOut>(_applicationUserResolver, _useCaseLogger, _validatorResolver);
    }
}
