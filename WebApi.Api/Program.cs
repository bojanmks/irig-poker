using FastEndpoints;
using WebApi.Api.Extensions;
using WebApi.Api.Hubs;
using WebApi.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.SetupApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseFastEndpoints(config =>
{
    config.Endpoints.AllowEmptyRequestDtos = true;
    config.Endpoints.RoutePrefix = "api";
});

app.MapHub<GameHub>("/hubs/game");

app.Run();
