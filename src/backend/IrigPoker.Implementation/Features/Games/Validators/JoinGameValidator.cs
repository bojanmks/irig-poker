using FluentValidation;
using IrigPoker.Application.Core.Localization;
using IrigPoker.Application.Features.Games.Commands;
using IrigPoker.Implementation.Core.Validation.Models;

namespace IrigPoker.Implementation.Features.Games.Validators;

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
