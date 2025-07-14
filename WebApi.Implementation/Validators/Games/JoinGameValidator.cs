using FluentValidation;
using WebApi.Application.Games;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Games;

namespace WebApi.Implementation.Validators.Games;

public class JoinGameValidator : BaseValidator<JoinGameUseCase>
{
    public JoinGameValidator(ITranslator translator, IGameExistsService gameExistsService) : base(translator)
    {
        RuleFor(x => x.Data.Username)
            .NotEmpty()
            .WithMessage(IsRequired())
            .MaximumLength(20)
            .WithMessage(MaxLength(20));

        RuleFor(x => x.Data.GameCode)
            .NotEmpty()
            .WithMessage(IsRequired())
            .MustAsync(gameExistsService.ExistsAsync)
            .WithMessage(T("gameNotFound"));
    }
}
