namespace WebApi.Common.DTO.Players;

public class PlayerDto
{
    public required string Username { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsDisconnected { get; set; }
}
