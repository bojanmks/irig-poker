using WebApi.Common.Core.Result.Models;

namespace WebApi.Application.Core.UseCases;

public class UseCaseSubscriberData<TData, TOut>
{
    public TData UseCaseData { get; set; }
    public Result<TOut> UseCaseResult { get; set; }
}
