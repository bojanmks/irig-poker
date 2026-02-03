using FluentValidation;

namespace WebApi.Application.Core.Validation;

public interface IValidatorResolver
{
    AbstractValidator<T> Resolve<T>();
}
