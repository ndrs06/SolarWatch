using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service;

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
    public async Task GetSolarWatch_ShouldReturnOk_WhenDbSunriseSunsetExists()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.Now;
        var dbSunriseSunset = new SunriseSunset();
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns(dbSunriseSunset);
        
        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), res.Result);
    }
    
    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenExceptionInGetOpenWeatherMapApiDataAsync()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ThrowsAsync(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnNotFound_WhenExceptionInGetSunriseSunsetApiDataAsync()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        var coordinates = new Coordinates();
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Returns(coordinates);
        _sunriseSunsetServiceMock.Setup(service => service.GetSunriseSunsetApiDataAsync(date, coordinates)).ThrowsAsync(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenExceptionInAddCityToDb()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        var coordinates = new Coordinates();
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Returns(coordinates);
        _cityServiceMock.Setup(service => service.ProcessCity("jsonData")).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenExceptionInAddSunriseSunsetToDb()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        var coordinates = new Coordinates();
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Returns(coordinates);
        _cityServiceMock.Setup(service => service.ProcessCity("jsonData")).Returns(new City());
        _sunriseSunsetServiceMock.Setup(service => service.GetSunriseSunsetApiDataAsync(date, coordinates)).ReturnsAsync("jsonData");
        _sunriseSunsetServiceMock.Setup(service => service.ProcessSunriseSunset("jsonData")).Returns(new SunriseSunset());
        _sunriseSunsetServiceMock.Setup(service => service.AddSunriseSunsetToDb(It.IsAny<SunriseSunset>())).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnOk_WhenSunriseSunsetDataProcessed()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        var coordinates = new Coordinates();
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Returns(coordinates);
        _cityServiceMock.Setup(service => service.ProcessCity("jsonData")).Returns(new City());
        _sunriseSunsetServiceMock.Setup(service => service.GetSunriseSunsetApiDataAsync(date, coordinates)).ReturnsAsync("jsonData");
        _sunriseSunsetServiceMock.Setup(service => service.ProcessSunriseSunset("jsonData")).Returns(new SunriseSunset());
        _sunriseSunsetServiceMock.Setup(service => service.AddSunriseSunsetToDb(It.IsAny<SunriseSunset>())).Verifiable();
        _sunriseSunsetServiceMock.Setup(service => service.ProcessSolarWatch("jsonData")).Returns(new SolarWatch());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenCityServiceThrowsException()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenExceptionInAddCityToDbAfterFetchingCoordinates()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        var coordinates = new Coordinates();
        _sunriseSunsetServiceMock.Setup(service =>  service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Returns(coordinates);
        _cityServiceMock.Setup(service => service.ProcessCity("jsonData")).Returns(new City());
        _cityServiceMock.Setup(service => service.AddCityToDb(It.IsAny<City>())).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnNotFound_WhenGeneralExceptionOccurs()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenAddCityToDbFails()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Returns(new Coordinates());
        _cityServiceMock.Setup(service => service.ProcessCity("jsonData")).Returns(new City());
        _cityServiceMock.Setup(service => service.AddCityToDb(It.IsAny<City>())).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenAddSunriseSunsetToDbFails()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        var coordinates = new Coordinates();
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.GetOpenWeatherMapApiDataAsync(cityName)).ReturnsAsync("jsonData");
        _cityServiceMock.Setup(service => service.ProcessCityCoordinates("jsonData")).Returns(coordinates);
        _cityServiceMock.Setup(service => service.ProcessCity("jsonData")).Returns(new City());
        _sunriseSunsetServiceMock.Setup(service => service.GetSunriseSunsetApiDataAsync(date, coordinates)).ReturnsAsync("jsonData");
        _sunriseSunsetServiceMock.Setup(service => service.ProcessSunriseSunset("jsonData")).Returns(new SunriseSunset());
        _sunriseSunsetServiceMock.Setup(service => service.AddSunriseSunsetToDb(It.IsAny<SunriseSunset>())).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }
}