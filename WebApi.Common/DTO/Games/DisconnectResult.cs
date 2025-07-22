namespace WebApi.Common.DTO.Games;

public class DisconnectResult
{
    public string? GameCode { get; set; }
    public string? ChangedAdminTo { get; set; }
    public bool HasGameEnded { get; set; }
}
