namespace WebApi.Common.Features.Games.Models;

public class Deck
{
    private readonly List<Card> _cards;
    private int _nextCardIndex;

    public Deck()
    {
        _cards = [];
        foreach (Suit suit in Enum.GetValues<Suit>())
        {
            foreach (Rank rank in Enum.GetValues<Rank>())
            {
                _cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        var cards = _cards.ToArray();
        Random.Shared.Shuffle(cards);
        _cards.Clear();
        _cards.AddRange(cards);
        _nextCardIndex = 0;
    }

    public Card DealOne()
    {
        if (_nextCardIndex >= _cards.Count)
        {
            throw new InvalidOperationException("The deck is empty");
        }

        return _cards[_nextCardIndex++];
    }

    public List<Card> Deal(int count)
    {
        var cards = new List<Card>(count);
        for (int i = 0; i < count; i++)
        {
            cards.Add(DealOne());
        }
        return cards;
    }

    public int RemainingCount => _cards.Count - _nextCardIndex;
}
