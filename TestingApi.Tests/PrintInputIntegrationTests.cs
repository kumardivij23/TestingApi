using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestingApi.Tests;

public class PrintInputIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PrintInputIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_EndpointIsAccessible_ReturnsSuccessStatusCode()
    {
        // Arrange
        var content = new StringContent(
            "{\"test\": true}",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/PrintInput", content);

        // Assert
        Assert.True(response.IsSuccessStatusCode,
            $"Expected success status code but got {response.StatusCode}");
    }

    [Fact]
    public async Task Get_ExistingEndpoint_StillWorks()
    {
        // Act
        var response = await _client.GetAsync("/PrintInput");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello welcome to the controller print", responseBody);
    }
}
