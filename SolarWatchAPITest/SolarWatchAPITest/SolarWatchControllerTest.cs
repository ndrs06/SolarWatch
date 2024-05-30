using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Model;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;

namespace SolarWatchAPITest;

public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISolarWatchDataProvider> _solarWatchDataProviderMock;
    private Mock<ICoordinatesProvider> _coordinatesProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private SolarWatchController _solarWatchController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _solarWatchDataProviderMock = new Mock<ISolarWatchDataProvider>();
        _coordinatesProviderMock = new Mock<ICoordinatesProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _solarWatchController = new SolarWatchController(_loggerMock.Object, _coordinatesProviderMock.Object, _solarWatchDataProviderMock.Object, _jsonProcessorMock.Object);
    }

    [Test]
    public async Task Get_ReturnsNotFoundIfCityNameIsInvalid()
    {
        // Arrange
        _coordinatesProviderMock.Setup(x => x.GetCoordinates(It.IsAny<string>())).Throws(new Exception());
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
        var coordinatesData = "{}";
        _coordinatesProviderMock.Setup(x => x.GetCoordinates(It.IsAny<string>())).ReturnsAsync(coordinatesData);
        _jsonProcessorMock.Setup(x => x.ProcessCoordinates(coordinatesData)).Throws<Exception>();
        
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
        _solarWatchDataProviderMock.Setup(x => x.GetCurrent(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<double>())).Throws(new Exception());
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
        var solarWatchData = "{}";
        _solarWatchDataProviderMock.Setup(x => x.GetCurrent(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(solarWatchData);
        _jsonProcessorMock.Setup(x => x.ProcessSolarWatch(solarWatchData)).Throws<Exception>();
        
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

        var expectedCoordinatesData = new Coordinates();
        var expectedSolarWatchData = new SolarWatch();
        var data = "{}";
        _coordinatesProviderMock.Setup(x => x.GetCoordinates(It.IsAny<string>())).ReturnsAsync(data);
        _solarWatchDataProviderMock
            .Setup(x => x.GetCurrent(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(data);
        _jsonProcessorMock.Setup(x => x.ProcessCoordinates(data)).Returns(expectedCoordinatesData);
        _jsonProcessorMock.Setup(x => x.ProcessSolarWatch(data)).Returns(expectedSolarWatchData);
        
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = await _solarWatchController.Get(city, date);
        
        // Arrange
        Assert.IsInstanceOf(typeof(OkObjectResult), res.Result);

    }
}