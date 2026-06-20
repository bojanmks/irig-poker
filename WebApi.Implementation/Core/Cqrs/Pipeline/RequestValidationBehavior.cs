using FluentValidation;
using MediatR;
using WebApi.Common.Core.Result.Models;

namespace WebApi.Implementation.Core.Cqrs.Pipeline;

public class RequestValidationBehavior<TRequest, TInner> : IPipelineBehavior<TRequest, Result<TInner>>
    where TRequest : IRequest<Result<TInner>>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Result<TInner>> Handle(TRequest request, RequestHandlerDelegate<Result<TInner>> next, CancellationToken cancellationToken)
    {
        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var fieldErrors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .Select(x => new FieldErrors
                    {
                        Field = x.Key,
                        Errors = x.Select(g => g.ErrorMessage)
                    });

                return Result<TInner>.ValidationError(fieldErrors: fieldErrors);
            }
        }

        return await next();
    }
}
