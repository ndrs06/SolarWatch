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
    public ActionResult<SolarWatch> Get(string city, DateTime date)
    {
        try
        {
            var coordinates = _jsonProcessor.ProcessCoordinates(_coordinatesProvider.GetCoordinates(city));
            var solarWatchData = _solarWatchDataProvider.GetCurrent(date, coordinates.Lat, coordinates.Lon);

            return Ok(_jsonProcessor.ProcessSolarWatch(solarWatchData));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound("Error getting data");
        }
    }
}
