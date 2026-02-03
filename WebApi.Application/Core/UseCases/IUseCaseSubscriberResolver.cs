namespace WebApi.Application.Core.UseCases;

public interface IUseCaseSubscriberResolver
{
    IEnumerable<IUseCaseSubscriber<TUseCase, TData, TOut>> ResolveAll<TUseCase, TData, TOut>() where TUseCase : UseCase<TData, TOut>;
}
