using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.DataAccess.Entities;

public class Game : SoftDeletableEntity
{
    public string Code { get; set; }
    public bool HasStarted { get; set; }
}