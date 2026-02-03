namespace WebApi.Api.Core.Modules;

public interface IModule
{
    public int Priority { get; }
    void RegisterServices(IServiceCollection services);
    void UseServices(WebApplication app);
}

public abstract class BaseModule : IModule
{
    public virtual int Priority => int.MaxValue;
    public virtual void RegisterServices(IServiceCollection services) { }

    public virtual void UseServices(WebApplication app) { }
}
