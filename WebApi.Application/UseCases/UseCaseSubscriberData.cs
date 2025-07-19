using WebApi.Common.DTO.Result;

namespace WebApi.Application.UseCases
{
    public class UseCaseSubscriberData<TData, TOut>
    {
        public TData UseCaseData { get; set; }
        public Result<TOut> Response { get; set; }
    }
}
