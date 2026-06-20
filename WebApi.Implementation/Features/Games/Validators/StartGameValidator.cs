using FluentValidation;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.AppSettings;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Commands;
using WebApi.Application.Features.Games.Services;
using WebApi.Implementation.Core.Validation.Models;

namespace WebApi.Implementation.Features.Games.Validators;

public class StartGameValidator : BaseValidator<StartGameCommand>
{
    public StartGameValidator(
        ITranslator translator,
        IApplicationUserResolver applicationUserResolver,
        IGetGameService getGameService,
        AppSettings appSettings
    ) : base(translator)
    {
        RuleFor(x => x)
            .CustomAsync(async (_, context, ct) =>
            {
                var applicationUser = await applicationUserResolver.ResolveAsync(ct);

                if (string.IsNullOrWhiteSpace(applicationUser.GameCode))
                {
                    context.AddFailure(T("user.notInGame"));
                    return;
                }

                var game = await getGameService.GetAsync(applicationUser.GameCode, ct);

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

                if (game.Players.Count < appSettings.MinPlayersPerGame)
                {
                    context.AddFailure(T("game.notEnoughPlayers"));
                    return;
                }

                if (game.Players.Count > appSettings.MaxPlayersPerGame)
                {
                    context.AddFailure(T("game.tooManyPlayers"));
                    return;
                }
            });
    }
}
