using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MinimalAPIService;
using MinimalAPIService.SimulationService;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// the client doesn't need to know the technology about the server.
builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

builder.Host.UseDefaultServiceProvider(config => config.ValidateOnBuild = true);

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddSerilog((sp, config) =>
{
    config
        .Enrich.FromLogContext()
        .Filter.ByExcluding("RequestPath like '/favicon.ico'")
        .Filter.ByExcluding("RequestPath like '/health%'")
        .Filter.ByExcluding("Uri like '%/health%'")
        .Filter.ByExcluding(ev => ev.MessageTemplate.Text.Equals("Saved {count} entities to in-memory store."))
        .ReadFrom.Configuration(sp.GetRequiredService<IConfiguration>())
        .WriteTo.Console();
});

builder.Services.ConfigureHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseSecurityHeaders(policies => policies
    .AddDefaultApiSecurityHeaders()
    .AddPermissionsPolicyWithDefaultSecureDirectives()
    // Adjust CSP for Developper Exception Page
    .AddContentSecurityPolicy(configure => configure.AddScriptSrc().Self().UnsafeInline()));

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async httpContext =>
        {
            var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
            if (pds == null || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
                await httpContext.Response.WriteAsync("Fallback: An error occurred.");
        });
    });
}

app.UseStatusCodePages(async statusCodeContext
    => await Results
        .Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
        .ExecuteAsync(statusCodeContext.HttpContext));

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Minimal API Service")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithTheme(ScalarTheme.Saturn)
            .WithDarkMode();
    }).AllowAnonymous();
}

#if DEBUG
app.MapGet("/exception", () =>
{
    throw new InvalidOperationException("Sample Exception");
});

app.MapGet("/status500", () =>
{
    return Results.StatusCode(500);
});
#endif

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

SimulationService.Register(app);

app.Run();