using Microsoft.Extensions.Configuration;
using MinimalAPIService.SimulationService;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSerilog((sp, config) =>
{
    config
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(sp.GetRequiredService<IConfiguration>())
    .WriteTo.Console();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Minimal API Service")
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
        .WithTheme(ScalarTheme.Saturn)
        .WithDarkMode();
    }).AllowAnonymous();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

SimulationService.Register(app);

app.Run();