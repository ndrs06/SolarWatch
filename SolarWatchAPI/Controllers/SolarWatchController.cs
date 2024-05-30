using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICoordinatesProvider _coordinatesProvider;
    private ISolarWatchDataProvider _solarWatchDataProvider;
    private IJsonProcessor _jsonProcessor;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICoordinatesProvider coordinatesProvider, ISolarWatchDataProvider solarWatchDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _coordinatesProvider = coordinatesProvider;
        _solarWatchDataProvider = solarWatchDataProvider;
        _jsonProcessor = jsonProcessor;
    }
    
    [HttpGet(Name = "SolarWatch")]
    public async Task<ActionResult<SolarWatch>> Get(string city, DateTime date)
    {
        Coordinates coordinates;
        try
        {
            var coordinatesData = await _coordinatesProvider.GetCoordinates(city);
            coordinates = _jsonProcessor.ProcessCoordinates(coordinatesData);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound($"Not found city with this name: {city}");
        }
        
        try
        {
            var solarWatchData = await _solarWatchDataProvider.GetCurrent(date, coordinates.Lat, coordinates.Lon);
            var solarWatch = _jsonProcessor.ProcessSolarWatch(solarWatchData);
            solarWatch.City = city;
            return Ok(solarWatch);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound("Error getting data");
        }
    }
}
