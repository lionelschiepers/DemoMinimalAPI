using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace MinimalAPIService.IntegrationTests;

public class APITests(CustomWebApplicationFactory<Program> _factory) :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false
    });

    [Fact]
    public async Task Status500_Returns500()
    {
        CancellationToken token = CancellationToken.None;
        var defaultPage = await _client.GetAsync("/status500", token);

        Assert.Equal(HttpStatusCode.InternalServerError, defaultPage.StatusCode);
    }

}