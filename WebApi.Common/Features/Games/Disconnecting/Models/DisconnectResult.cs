using WebApi.Common.Features.Games.Models;

namespace WebApi.Common.Features.Games.Disconnecting.Models;

public class DisconnectResult
{
    public string? GameCode { get; set; }
    public string? PlayerId { get; set; }
    public string? ChangedAdminTo { get; set; }
    public bool HasGameEnded { get; set; } = false;
    public string? WinnerPlayerId { get; set; }
    public string? WinnerUsername { get; set; }
    public PublicGameState? UpdatedGameState { get; set; }
}
