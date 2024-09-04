using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service;
using Xunit;

namespace SolarWatchAPIIntegrationTest;

[Collection("IntegrationTest")]

public class SolarWatchControllerIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SolarWatchWebApplicationFactory _app;

    public SolarWatchControllerIntegrationTest()
    {
        _app = new SolarWatchWebApplicationFactory();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task GetSolarWatch_ReturnsOk_WhenCityAndDateExists()
    {
        // Arrange
        var cityName = "Miskolc";
        var date = DateTime.UtcNow.Date;
        
        // Act
        var response = await _client.GetAsync($"/api/solar-watch?cityName={cityName}&date={date:yyyy-MM-dd}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var solarWatch = JsonSerializer.Deserialize<SolarWatch>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(solarWatch);
        Assert.Equal("Miskolc", solarWatch.City);
        Assert.Equal(date, solarWatch.Date);
        Assert.Equal(new TimeOnly(6, 0), solarWatch.Sunrise);
        Assert.Equal(new TimeOnly(18, 0), solarWatch.Sunset);
    }
}