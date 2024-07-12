using System.Net.Http.Json;
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
    private readonly SolarWatchWebApplicationFactory _app;
    private readonly HttpClient _client;
    private readonly Mock<ILogger<SolarWatchController>> _loggerMock;
    private readonly Mock<ICityService> _cityServiceMock;
    private readonly Mock<ISunriseSunsetService> _sunriseSunsetServiceMock;

    public SolarWatchControllerIntegrationTest()
    {
        _app = new SolarWatchWebApplicationFactory();
        _client = _app.CreateClient();
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _cityServiceMock = new Mock<ICityService>();
        _sunriseSunsetServiceMock = new Mock<ISunriseSunsetService>();
        
        ConfigureMockServices();
    }
    
    private void ConfigureMockServices()
    {

        _loggerMock.Setup(log => log.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<object>(),
            It.IsAny<Exception>(),
            (Func<object, Exception, string>)It.IsAny<object>()));

        
        _cityServiceMock.Setup(service => service.GetByName(It.IsAny<string>()))
            .Returns((string cityName) => new City
            {
                Name = cityName,
                Lat = 47.4979,
                Lon = 19.0402
            });

        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns((string cityName, DateTime date) => new SunriseSunset
            {
                CityName = cityName,
                Date = date,
                Sunrise = new TimeOnly(06, 20),
                Sunset = new TimeOnly(14, 54)
            });
    }

    [Fact]
    public async Task TestEndPoint()
    {
        // Arrange
        var cityName = "Budapest";
        var date = new DateTime(2012, 12, 12);
        
        // Act
        var response = await _client.GetAsync($"api/SolarWatch?cityName={cityName}&date={date}");

        // Assert
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<SolarWatch>();
        
        Assert.NotNull(data);
        Assert.Equal(cityName, data.City);
        Assert.Equal(date, data.Date);
        Assert.Equal(new TimeOnly(06, 20), data.Sunrise);
        Assert.Equal(new TimeOnly(14, 54), data.Sunset);
    }
}