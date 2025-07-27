using FluentValidation;
using WebApi.Application.AppSettings;
using WebApi.Application.Games;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Games;

namespace WebApi.Implementation.Validators.Games;

public class JoinGameValidator : BaseValidator<JoinGameUseCase>
{
    public JoinGameValidator(
        ITranslator translator,
        IGetGameService getGameService,
        AppSettings appSettings
    ) : base(translator)
    {
        RuleFor(x => x.Data.Username)
            .NotEmpty()
            .WithMessage(IsRequired())
            .MaximumLength(20)
            .WithMessage(MaxLength(20));

        RuleFor(x => x.Data.GameCode)
            .NotEmpty()
            .WithMessage(IsRequired())
            .CustomAsync(async (gameCode, context, ct) =>
            {
                var game = await getGameService.GetAsync(gameCode, ct);

                if (game is null)
                {
                    context.AddFailure(T("game.notFound"));
                    return;
                }

                if (game.HasStarted)
                {
                    context.AddFailure(T("game.alreadyStarted"));
                    return;
                }

                if (game.Players.Count >= appSettings.MaxPlayersPerGame)
                {
                    context.AddFailure(T("game.isFull"));
                    return;
                }
            });
    }
}
