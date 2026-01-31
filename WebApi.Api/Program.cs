using WebApi.Api.Core;
using WebApi.Application.Core.AppSettings;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddSingleton(appSettings);
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

var modules = typeof(Program).Assembly.GetTypes().Where(p =>
    typeof(IModule).IsAssignableFrom(p)
    && p.IsClass
    && !p.IsAbstract
)
.Select(x => (IModule)Activator.CreateInstance(x)!)
.OrderBy(x => x.Priority)
.ToArray();

foreach (var module in modules)
{
    module.RegisterServices(builder.Services);
}

var app = builder.Build();

foreach (var module in modules)
{
    module.UseServices(app);
}

app.Run();