# irig-poker

A web-based multiplayer poker game with a bluffing mechanic. Built with ASP.NET Core 10 (C#) and React 19 + TypeScript.

## Game Rules

### Overview

This is a custom poker variant. Standard poker hand rankings apply with these differences:

- **Straight** is weaker than **Three of a Kind**
- **Flush does not exist** — suit/color is disregarded, *except* in Straight Flush and Royal Flush (which require same suit)

### Hand Rankings (strongest to weakest)

1. Royal Flush
2. Straight Flush
3. Four of a Kind
4. Full House
5. Three of a Kind
6. Straight
7. Two Pair
8. One Pair
9. High Card

### How a Round Works

1. **Deal** — Each player receives **one card**.
2. **Claim** — The active player claims what poker hand they hold (they may bluff).
3. **Challenge** — The next player (in turn order) must either:
   - **Up the bet** — claim a stronger hand than the previous claim, **or**
   - **Call bluff** — say the previous player is lying.
4. **Resolution** — When a bluff is called, **all players' cards are revealed and combined**. If the claimed hand exists within the combined pool of cards, the claimant is truthful. Otherwise, they were bluffing.
5. **Penalty** — The losing player (the bluffer if caught, or the caller if the claim was truthful) **gains one card**.
6. A player is **eliminated** when they accumulate **7 cards** (may vary depending on player count).
7. **Winner** — The last player remaining wins.