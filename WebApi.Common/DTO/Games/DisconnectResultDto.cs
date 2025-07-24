namespace WebApi.Common.DTO.Games;

public class DisconnectResultDto
{
    public string? GameCode { get; set; }
    public string? ChangedAdminTo { get; set; }
    public bool HasGameEnded { get; set; }
}
