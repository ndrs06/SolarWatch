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
    public async Task GetSolarWatch_ShouldReturnOK_WhenDbSunriseSunsetDoesNotExistsButTheDbCityDoes()
    {
        // Arrange
        var cityName = "TestCity";
        var dbCity = new City { Name = cityName };
        var dbSunriseSunset = new SunriseSunset { CityName = dbCity.Name };
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns(dbCity);
        _sunriseSunsetServiceMock.Setup(service => service.AddSunriseSunsetToDb(cityName, date));
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns(dbSunriseSunset);

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), res.Result);
    }
    
    [Test]
    public async Task GetSolarWatch_ShouldReturnOK_WhenDbSunriseSunsetAndDbCityDoesNotExists()
    {
        // Arrange
        var cityName = "TestCity";
        var dbCity = new City { Name = cityName };
        var dbSunriseSunset = new SunriseSunset { CityName = dbCity.Name };
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.AddCityToDb(cityName));
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns(dbCity);
        _sunriseSunsetServiceMock.Setup(service => service.AddSunriseSunsetToDb(cityName, date));
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns(dbSunriseSunset);

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenAddCityToDbFails()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _cityServiceMock.Setup(service => service.AddCityToDb(cityName)).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(ObjectResult), res.Result);
    }

    [Test]
    public async Task GetSolarWatch_ShouldReturnBadRequest_WhenAddSunriseSunsetToDbFails()
    {
        // Arrange
        var cityName = "TestCity";
        var date = DateTime.UtcNow;
        _sunriseSunsetServiceMock.Setup(service => service.GetByCityNameAndDate(cityName, date)).Returns((SunriseSunset)null!);
        _cityServiceMock.Setup(service => service.GetByName(cityName)).Returns((City)null!);
        _sunriseSunsetServiceMock.Setup(service => service.AddSunriseSunsetToDb(cityName, date)).Throws(new Exception());

        // Act
        var res = await _solarWatchController.GetSolarWatch(cityName, date);

        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), res.Result);
    }
}