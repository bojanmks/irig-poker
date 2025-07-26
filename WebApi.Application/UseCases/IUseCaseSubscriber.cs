namespace WebApi.Application.UseCases
{
    public interface IUseCaseSubscriber<TUseCase, TData, TOut> where TUseCase : UseCase<TData, TOut>
    {
        Task ExecuteAsync(UseCaseSubscriberData<TData, TOut> data, CancellationToken cancellationToken = default);
    }
}
