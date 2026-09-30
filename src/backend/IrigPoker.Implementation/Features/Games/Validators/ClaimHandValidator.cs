using FluentValidation;
using IrigPoker.Application.Core.Localization;
using IrigPoker.Application.Features.Games.Commands;
using IrigPoker.Common.Features.Games.Models;
using IrigPoker.Implementation.Core.Validation.Models;

namespace IrigPoker.Implementation.Features.Games.Validators;

public class ClaimHandValidator : BaseValidator<ClaimHandCommand>
{
    private static readonly HandType[] SingleRankHandTypes =
    [
        HandType.HighCard,
        HandType.OnePair,
        HandType.Straight,
        HandType.ThreeOfAKind,
        HandType.FourOfAKind,
        HandType.StraightFlush
    ];

    private static readonly HandType[] TwoRankHandTypes =
    [
        HandType.TwoPair,
        HandType.FullHouse
    ];

    public ClaimHandValidator(
        ITranslator translator
    ) : base(translator)
    {
        RuleFor(x => x.Data.ClaimedHand)
            .IsInEnum()
            .WithMessage(InvalidClaim());

        RuleFor(x => x.Data.Suit)
            .IsInEnum()
            .When(x => x.Data.Suit.HasValue)
            .WithMessage(InvalidClaim());

        RuleFor(x => x.Data.Ranks)
            .NotNull()
            .WithMessage(InvalidClaim());

        RuleForEach(x => x.Data.Ranks)
            .IsInEnum()
            .WithMessage(InvalidClaim());

        RuleFor(x => x)
            .Must(HaveExpectedRankCount)
            .WithMessage(InvalidClaim());

        RuleFor(x => x)
            .Must(HaveDistinctRanks)
            .When(x => x.Data.Ranks is not null)
            .WithMessage(InvalidClaim());

        RuleFor(x => x)
            .Must(HaveValidStraightTopRank)
            .WithMessage(InvalidClaim());

        RuleFor(x => x.Data.Suit)
            .NotNull()
            .When(x => x.Data.ClaimedHand == HandType.StraightFlush)
            .WithMessage(InvalidClaim());
    }

    private static bool HaveExpectedRankCount(ClaimHandCommand command)
    {
        var ranks = command.Data.Ranks;
        if (ranks is null)
        {
            return false;
        }

        if (SingleRankHandTypes.Contains(command.Data.ClaimedHand))
        {
            return ranks.Count == 1;
        }

        if (TwoRankHandTypes.Contains(command.Data.ClaimedHand))
        {
            return ranks.Count == 2;
        }

        return command.Data.ClaimedHand == HandType.RoyalFlush && ranks.Count == 0;
    }

    private static bool HaveDistinctRanks(ClaimHandCommand command)
    {
        var ranks = command.Data.Ranks;
        return ranks is null || ranks.Distinct().Count() == ranks.Count;
    }

    private static bool HaveValidStraightTopRank(ClaimHandCommand command)
    {
        var data = command.Data;

        if (data.ClaimedHand is not (HandType.Straight or HandType.StraightFlush))
        {
            return true;
        }

        return data.Ranks is { Count: > 0 } && HandEvaluator.GetStraightRanks(data.Ranks[0]).Count > 0;
    }

    private string InvalidClaim()
    {
        return T("game.invalidClaim");
    }
}
