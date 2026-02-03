using Newtonsoft.Json;
using System.Net;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.Logging;
using WebApi.Application.Core.Logging.LoggerData;
using WebApi.Application.Core.UseCases;
using WebApi.Application.Core.Validation;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Core.UseCases.Execution;

public class UseCaseExecutor<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
{
    private readonly IApplicationUserResolver _applicationUserResolver;
    private readonly IUseCaseLogger _useCaseLogger;
    private readonly IUseCaseSubscriberResolver _subscriberResolver;
    private readonly IValidatorResolver _validatorResolver;

    public UseCaseExecutor(
        IApplicationUserResolver applicationUserResolver,
        IUseCaseLogger useCaseLogger,
        IUseCaseSubscriberResolver subscriberResolver,
        IValidatorResolver validatorResolver
    )
    {
        _applicationUserResolver = applicationUserResolver;
        _useCaseLogger = useCaseLogger;
        _subscriberResolver = subscriberResolver;
        _validatorResolver = validatorResolver;
    }

    public async Task<Result<TOut>> ExecuteAsync(TUseCase useCase, UseCaseHandler<TUseCase, TData, TOut> handler, CancellationToken cancellationToken = default)
    {
        var validationResponse = await ValidateAndLog(useCase, cancellationToken);

        if (!validationResponse.IsSuccess)
        {
            return validationResponse;
        }

        var response = await handler.HandleAsync(useCase, cancellationToken);

        if (response.IsSuccess)
        {
            await ExecuteUseCaseSubscribers(useCase, response, cancellationToken);
        }

        return response;
    }

    private async Task<Result<TOut>> ValidateAndLog(TUseCase useCase, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _applicationUserResolver.ResolveAsync(cancellationToken);

        var isAuthorized = applicationUser.AllowedUseCases.Contains(useCase.Id);

        var log = new UseCaseLoggerData
        {
            UseCaseId = useCase.Id,
            IsAuthorized = isAuthorized,
            ExecutionDateTime = DateTime.UtcNow,
            Data = JsonConvert.SerializeObject(useCase.Data)
        };

        await _useCaseLogger.Log(log);

        if (!isAuthorized)
        {
            var errorResult = Result<TOut>.Error()
                .WithHttpStatusCode((int)HttpStatusCode.Forbidden);

            return errorResult;
        }

        var validator = _validatorResolver.Resolve<TUseCase>();

        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(useCase, cancellationToken);

            if (!validationResult.IsValid)
            {
                var fieldErrors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .Select(x => new FieldErrors
                    {
                        Field = x.Key,
                        Errors = x.Select(g => g.ErrorMessage)
                    });

                return Result<TOut>.ValidationError(fieldErrors: fieldErrors);
            }
        }

        return Result<TOut>.Success();
    }

    private async Task ExecuteUseCaseSubscribers(TUseCase useCase, Result<TOut> useCaseResponse, CancellationToken cancellationToken = default)
    {
        var subscribers = _subscriberResolver.ResolveAll<TUseCase, TData, TOut>();

        if (subscribers is null)
        {
            return;
        }

        var subscriberData = new UseCaseSubscriberData<TData, TOut>
        {
            UseCaseData = useCase.Data,
            UseCaseResult = useCaseResponse
        };

        foreach (var subscriber in subscribers)
        {
            await subscriber.ExecuteAsync(subscriberData, cancellationToken);
        }
    }
}