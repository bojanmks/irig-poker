namespace WebApi.Common.Features.Games.Models;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 12,
    Queen = 13,
    King = 14,
    Ace = 99
}

public record Card(Suit Suit, Rank Rank);
