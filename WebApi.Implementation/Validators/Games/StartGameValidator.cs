using FluentValidation;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Games;
using WebApi.Application.Localization;
using WebApi.Application.UseCases.Games;

namespace WebApi.Implementation.Validators.Games;

public class StartGameValidator : BaseValidator<StartGameUseCase>
{
    public StartGameValidator(ITranslator translator, IApplicationUserResolver applicationUserResolver, IGetGameService getGameService) : base(translator)
    {
        RuleFor(x => x.Data)
            .MustAsync(async (_, ct) =>
            {
                var applicationUser = await applicationUserResolver.ResolveAsync(ct);
                var game = await getGameService.GetAsync(applicationUser.GameCode, ct);

                return !game!.HasStarted;
            })
            .WithMessage(T("gameHasAlreadyStarted"));
    }
}
