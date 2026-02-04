namespace WebApi.Application.Core.UseCases;

public interface IUseCase<TData, TOut> : IIdentifyable
{
    TData Data { get; }
}

public interface IIdentifyable
{
    string Id { get; }
}
