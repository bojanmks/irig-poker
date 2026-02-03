namespace WebApi.Application.Core.UseCases;

public interface IUseCaseHandlerResolver
{
    UseCaseHandler<TUseCase, TData, TOut> Resolve<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>;
}
