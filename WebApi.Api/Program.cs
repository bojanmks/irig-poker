using FastEndpoints;
using WebApi.Api.Extensions;
using WebApi.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.SetupApplication();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseFastEndpoints(config =>
{
    config.Endpoints.AllowEmptyRequestDtos = true;
    config.Endpoints.RoutePrefix = "api";
});

app.Run();
