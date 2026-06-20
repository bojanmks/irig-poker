namespace WebApi.Common.Features.Players.Models;

public class PlayerDto
{
    public required string PlayerId { get; init; }
    public required string Username { get; init; }
    public bool IsAdmin { get; private set; }

    public void SetIsAdmin(bool value)
    {
        IsAdmin = value;
    }
}
