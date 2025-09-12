namespace MinimalAPIService.SimulationService
{
    public class SimulationService
    {
        public static void Register(WebApplication app)
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            app.MapGet("/weatherforecast", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new
                    {
                        Id = index
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithDescription("Gets the weather forecast for the next 5 days.");
        }
    }
}
