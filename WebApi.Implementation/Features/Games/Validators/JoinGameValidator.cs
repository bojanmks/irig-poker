using FluentValidation;
using WebApi.Application.Core.AppSettings;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Services;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Implementation.Core.Validation.Models;

namespace WebApi.Implementation.Features.Games.Validators;

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
