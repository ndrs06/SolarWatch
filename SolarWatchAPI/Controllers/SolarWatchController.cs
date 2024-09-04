using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;
using SolarWatchAPI.Service;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("api/solar-watch")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICityService _cityService;
    private readonly ISunriseSunsetService _sunriseSunsetService;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICityService cityService, ISunriseSunsetService sunriseSunsetService)
    {
        _logger = logger;
        _cityService = cityService;
        _sunriseSunsetService = sunriseSunsetService;
    }
    
    [HttpGet(Name = "solar-watch")]
    public async Task<ActionResult<SolarWatch>> GetSolarWatch(string cityName, DateTime date)
    {
        try
        {
            var dbSunriseSunset = _sunriseSunsetService.GetByCityNameAndDate(cityName, date);

            if (dbSunriseSunset != null)
            {
                return Ok(new SolarWatch
                {
                    City = dbSunriseSunset.CityName,
                    Date = dbSunriseSunset.Date,
                    Sunrise = dbSunriseSunset.Sunrise,
                    Sunset = dbSunriseSunset.Sunset
                });
            }

            var dbCity = _cityService.GetByName(cityName);
            if (dbCity != null)
            {
                _logger.LogInformation("Coordinates set from DB");
            }
            else
            {
                _logger.LogInformation($"DB does not contain city with this name: {cityName}");

                try
                {
                    await _cityService.AddCityToDb(cityName);
                    _logger.LogInformation("City {CityName} added to DB", cityName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(500, $"Failed to add city to the database: {e.Message}");
                }
                    
                dbCity = _cityService.GetByName(cityName);
                if (dbCity == null)
                {
                    _logger.LogError($"Failed to retrieve city {cityName} after adding to DB");
                    return BadRequest($"City {cityName} could not be retrieved after adding to DB.");
                }
                _logger.LogInformation($"City: {cityName} added to DB");
            }
            
            try
            {
                await _sunriseSunsetService.AddSunriseSunsetToDb(cityName, date);
                _logger.LogInformation($"SunriseSunset with date: {date} added to {cityName} in DB");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest($"Failed to add sunrise/sunset data to DB: {e.Message}");
            }
            
            dbSunriseSunset = _sunriseSunsetService.GetByCityNameAndDate(cityName, date);
            
            return Ok(new SolarWatch
            {
                City = dbSunriseSunset.CityName,
                Date = dbSunriseSunset.Date,
                Sunrise = dbSunriseSunset.Sunrise,
                Sunset = dbSunriseSunset.Sunset
            });
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return NotFound($"Failed to get sunrise/sunset data from DB: {e.Message}");
        }
    }
}