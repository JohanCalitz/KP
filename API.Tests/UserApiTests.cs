using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using API.Tests;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using API.KP.Common;
using API.Models;
using FluentAssertions;
public class UserApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient client;
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public UserApiTests(CustomWebApplicationFactory factory)
    {
        this.client = factory.CreateClient();
    }

    private async Task AuthenticateAsync()
    {
        var loginPayload = new
        {
            Username = "admin",
            Password = "YourPassword123"
        };

        var response = await client.PostAsJsonAsync("/login", loginPayload);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>(jsonOptions);
        var tokenModel = JsonSerializer.Deserialize<TokenModel>(result?.Data?.ToString() ?? "", jsonOptions);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel?.Token);
    }

    [Fact]
    public async Task GetDashboard_ShouldReturnData_WhenAuthenticated()
    {
        await AuthenticateAsync();

        var response = await client.GetAsync("/api/User/Dashboard");
        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>(jsonOptions);
        apiResponse.Should().NotBeNull();
        apiResponse?.ErrorMessage.Should().BeEmpty();
        apiResponse?.Data.Should().NotBeNull();
    }
}
