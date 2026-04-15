using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestingApi.Tests;

public class PrintInputControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PrintInputControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_WithValidJsonObject_ReturnsOkWithSameBody()
    {
        // Arrange
        var input = new { message = "hello", number = 42 };
        var content = new StringContent(
            JsonSerializer.Serialize(input),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/PrintInput", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        using var responseDoc = JsonDocument.Parse(responseBody);
        var root = responseDoc.RootElement;

        Assert.Equal("hello", root.GetProperty("message").GetString());
        Assert.Equal(42, root.GetProperty("number").GetInt32());
    }

    [Fact]
    public async Task Post_WithJsonArray_ReturnsOkWithSameArray()
    {
        // Arrange
        var input = new[] { 1, 2, 3 };
        var content = new StringContent(
            JsonSerializer.Serialize(input),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/PrintInput", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        using var responseDoc = JsonDocument.Parse(responseBody);
        var root = responseDoc.RootElement;

        Assert.Equal(JsonValueKind.Array, root.ValueKind);
        Assert.Equal(3, root.GetArrayLength());
        Assert.Equal(1, root[0].GetInt32());
        Assert.Equal(2, root[1].GetInt32());
        Assert.Equal(3, root[2].GetInt32());
    }

    [Fact]
    public async Task Post_WithNestedJson_ReturnsExactStructure()
    {
        // Arrange
        var input = new
        {
            user = new
            {
                name = "Divij",
                roles = new[] { "admin", "dev" }
            }
        };
        var content = new StringContent(
            JsonSerializer.Serialize(input),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/PrintInput", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        using var responseDoc = JsonDocument.Parse(responseBody);
        var root = responseDoc.RootElement;

        var user = root.GetProperty("user");
        Assert.Equal("Divij", user.GetProperty("name").GetString());

        var roles = user.GetProperty("roles");
        Assert.Equal(2, roles.GetArrayLength());
        Assert.Equal("admin", roles[0].GetString());
        Assert.Equal("dev", roles[1].GetString());
    }

    [Fact]
    public async Task Post_WithEmptyObject_ReturnsOkWithEmptyObject()
    {
        // Arrange
        var content = new StringContent(
            "{}",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/PrintInput", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        using var responseDoc = JsonDocument.Parse(responseBody);
        var root = responseDoc.RootElement;

        Assert.Equal(JsonValueKind.Object, root.ValueKind);
        Assert.Equal(0, root.EnumerateObject().Count());
    }

    [Fact]
    public async Task Post_WithStringValue_ReturnsOkWithSameString()
    {
        // Arrange
        var content = new StringContent(
            "\"just a string\"",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/PrintInput", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        using var responseDoc = JsonDocument.Parse(responseBody);
        var root = responseDoc.RootElement;

        Assert.Equal(JsonValueKind.String, root.ValueKind);
        Assert.Equal("just a string", root.GetString());
    }

    [Fact]
    public async Task Post_WithSpecialCharacters_ReturnsOkWithSameBody()
    {
        // Arrange
        var input = new { text = "héllo wörld! @#$%^&*() café naïve résumé 日本語 中文 🚀" };
        var content = new StringContent(
            JsonSerializer.Serialize(input),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/PrintInput", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        using var responseDoc = JsonDocument.Parse(responseBody);
        var root = responseDoc.RootElement;

        Assert.Equal("héllo wörld! @#$%^&*() café naïve résumé 日本語 中文 🚀",
            root.GetProperty("text").GetString());
    }
}
