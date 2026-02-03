using WebApi.Common.Core.Result.Models;

namespace WebApi.Application.Core.UseCases;

public abstract class UseCaseHandler<TUseCase, TData, TOut> : IUseCaseHandlerBase where TUseCase : UseCase<TData, TOut>
{
    public abstract Task<Result<TOut>> HandleAsync(TUseCase useCase, CancellationToken cancellationToken = default);
}

public interface IUseCaseHandlerBase { }
