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
    private Mock<ICoordinatesProvider> _coordinateProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private SolarWatchController _solarWatchController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _solarWatchDataProviderMock = new Mock<ISolarWatchDataProvider>();
        _coordinateProviderMock = new Mock<ICoordinatesProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _solarWatchController = new SolarWatchController(_loggerMock.Object, _coordinateProviderMock.Object, _solarWatchDataProviderMock.Object, _jsonProcessorMock.Object);
    }

    [Test]
    public void Get_ReturnsNotFoundIfCityNameIsInvalid()
    {
        // Arrange
        _coordinateProviderMock.Setup(x => x.GetCoordinates(It.IsAny<string>())).Throws(new Exception());
        var city = "kdjhfélsjhédg";
        var date = DateTime.Today;
        
        // Act
        var res = _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }
    
    [Test]
    public void Get_ReturnsNotFoundIfCoordinateDataIsInvalid()
    {
        // Arrange
        var coordinateData = "{}";
        _coordinateProviderMock.Setup(x => x.GetCoordinates(It.IsAny<string>())).Returns(coordinateData);
        _jsonProcessorMock.Setup(x => x.ProcessCoordinates(coordinateData)).Throws<Exception>();
        
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }
    
    [Test]
    public void Get_ReturnsNotFoundIfCoordinatesIsInvalid()
    {
        // Arrange
        _solarWatchDataProviderMock.Setup(x => x.GetCurrent(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<double>())).Throws(new Exception());
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }
    
    [Test]
    public void Get_ReturnsNotFoundIfSolarWatchDataIsInvalid()
    {
        // Arrange
        var solarWatchData = "{}";
        _solarWatchDataProviderMock.Setup(x => x.GetCurrent(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<double>())).Returns(solarWatchData);
        _jsonProcessorMock.Setup(x => x.ProcessSolarWatch(solarWatchData)).Throws<Exception>();
        
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = _solarWatchController.Get(city, date);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), res.Result);
    }

    [Test]
    public void Get_ReturnsOkIfAllDataAreValid()
    {
        // Arrange

        var expectedCoordinatesData = new Coordinates();
        var expectedSolarWatchData = new SolarWatch();
        var data = "{}";
        _coordinateProviderMock.Setup(x => x.GetCoordinates(It.IsAny<string>())).Returns(data);
        _solarWatchDataProviderMock
            .Setup(x => x.GetCurrent(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<double>())).Returns(data);
        _jsonProcessorMock.Setup(x => x.ProcessCoordinates(data)).Returns(expectedCoordinatesData);
        _jsonProcessorMock.Setup(x => x.ProcessSolarWatch(data)).Returns(expectedSolarWatchData);
        
        var city = "Miskolc";
        var date = DateTime.Today;
        
        // Act
        var res = _solarWatchController.Get(city, date);
        
        // Arrange
        Assert.IsInstanceOf(typeof(OkObjectResult), res.Result);

    }
}