using FluentValidation;
using IrigPoker.Application.Core.Localization;
using IrigPoker.Application.Features.Games.Queries;
using IrigPoker.Implementation.Core.Validation.Models;

namespace IrigPoker.Implementation.Features.Games.Validators;

public class GetGameStatusValidator : BaseValidator<GetGameStatusQuery>
{
    public GetGameStatusValidator(
        ITranslator translator
    ) : base(translator)
    {
        RuleFor(x => x.GameCode)
            .NotEmpty()
            .WithMessage(IsRequired());
    }
}