namespace WebApi.Common.Features.Games.Disconnecting.Models;

public class DisconnectResultDto
{
    public string? GameCode { get; set; }
    public string? PlayerId { get; set; }
    public string? ChangedAdminTo { get; set; }
    public bool HasGameEnded { get; set; } = false;
}
