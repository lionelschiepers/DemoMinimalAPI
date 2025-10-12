namespace MinimalAPIService.HelloWorld
{
    public static class HelloWorldAPI
    {
        public static void Register(WebApplication app)
        {
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
