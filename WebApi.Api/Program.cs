using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using WebApi.Api.Core.ErrorHandling.Middleware;
using WebApi.Api.Core.Hubs.Filters;
using WebApi.Api.Core.Reflection.Extensions;
using WebApi.Api.Features.Games.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.SetupApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR(options =>
{
    options.AddFilter<GlobalHubFilter>();
});

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
