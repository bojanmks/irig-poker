namespace WebApi.Common.Features.Players.Models;

public class PlayerDto
{
    public required string Username { get; set; }
    public bool IsAdmin { get; set; }
}
