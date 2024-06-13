using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Model;
using SolarWatchAPI.Service;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;

namespace SolarWatchAPITest;

public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISunriseSunsetService> _sunriseSunsetServiceMock;
    private Mock<ICityService> _cityServiceMock;
    private SolarWatchController _solarWatchController;
    

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _sunriseSunsetServiceMock = new Mock<ISunriseSunsetService>();
        _cityServiceMock = new Mock<ICityService>();
        _solarWatchController = new SolarWatchController(_loggerMock.Object, _cityServiceMock.Object, _sunriseSunsetServiceMock.Object);
    }

    [Test]
    public async Task Get_ReturnsNotFoundIfCityNameIsInvalid()
    {
        // Arrange
        _cityServiceMock.Setup(x => x.GetCityCoordinatesAsync(It.IsAny<string>())).Throws(new Exception());
        var city = "kdjhfélsjhédg";
        var date = DateTime.Today;
        
        // Act
        var res = await _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }
    
    [Test]
    public async Task Get_ReturnsNotFoundIfCoordinateDataIsInvalid()
    {
        // Arrange
        var coordinates = new Coordinates();
        _cityServiceMock.Setup(x => x.GetCityCoordinatesAsync(It.IsAny<string>())).ReturnsAsync(coordinates);
        
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = await _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }
    
    [Test]
    public async Task Get_ReturnsNotFoundIfCoordinatesIsInvalid()
    {
        // Arrange
        _sunriseSunsetServiceMock.Setup(x => x.GetSolarWatchAsync(It.IsAny<DateTime>(), It.IsAny<Coordinates>())).Throws(new Exception());
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = await _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }
    
    [Test]
    public async Task Get_ReturnsNotFoundIfSolarWatchDataIsInvalid()
    {
        // Arrange
        var solarWatch = new SolarWatch();
        _sunriseSunsetServiceMock.Setup(x => x.GetSolarWatchAsync(It.IsAny<DateTime>(), It.IsAny<Coordinates>())).ReturnsAsync(solarWatch);
        
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = await _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }

    [Test]
    public async Task Get_ReturnsOkIfAllDataAreValid()
    {
        // Arrange

        var expectedCoordinates = new Coordinates();
        var expectedSolarWatch = new SolarWatch();
        var data = "{}";
        _cityServiceMock.Setup(x => x.GetCityCoordinatesAsync(It.IsAny<string>())).ReturnsAsync(expectedCoordinates);
        _sunriseSunsetServiceMock
            .Setup(x => x.GetSolarWatchAsync(It.IsAny<DateTime>(), It.IsAny<Coordinates>())).ReturnsAsync(expectedSolarWatch);
        
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = await _solarWatchController.Get(city, date);
        
        // Arrange
        Assert.IsInstanceOf(typeof(OkObjectResult), res.Result);

    }
}