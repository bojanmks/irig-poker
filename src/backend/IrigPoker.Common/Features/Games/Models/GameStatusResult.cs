namespace IrigPoker.Common.Features.Games.Models;

public record GameStatusResult(
    bool Exists,
    bool HasStarted
);
