using FluentValidation;
using WebApi.Application.Core.ApplicationUsers;
using WebApi.Application.Core.AppSettings;
using WebApi.Application.Core.Localization;
using WebApi.Application.Features.Games.Services;
using WebApi.Application.Features.Games.UseCases;
using WebApi.Implementation.Core.Validation.Models;

namespace WebApi.Implementation.Features.Games.Validators;

public class StartGameValidator : BaseValidator<StartGameUseCase>
{
    public StartGameValidator(
        ITranslator translator,
        IApplicationUserResolver applicationUserResolver,
        IGetGameService getGameService,
        AppSettings appSettings
    ) : base(translator)
    {
        RuleFor(x => x.Data)
            .CustomAsync(async (_, context, ct) =>
            {
                var applicationUser = await applicationUserResolver.ResolveAsync(ct);
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
