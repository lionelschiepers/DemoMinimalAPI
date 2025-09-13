using MinimalAPIService.SimulationService;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddSerilog((sp, config) =>
{
    config
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(sp.GetRequiredService<IConfiguration>())
    .WriteTo.Console();
});

var app = builder.Build();

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