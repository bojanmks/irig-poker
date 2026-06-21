using FluentValidation;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Commands;
using WebApi.Implementation.Core.Validation.Models;

namespace WebApi.Implementation.Features.Games.Validators;

public class JoinGameValidator : BaseValidator<JoinGameCommand>
{
    public JoinGameValidator(
        ITranslator translator
    ) : base(translator)
    {
        RuleFor(x => x.Data.Username)
            .NotEmpty()
            .WithMessage(IsRequired())
            .MaximumLength(20)
            .WithMessage(MaxLength(20));

        RuleFor(x => x.Data.GameCode)
            .NotEmpty()
            .WithMessage(IsRequired());
    }
}
