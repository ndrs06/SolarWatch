using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;
using SolarWatchAPI.Service;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    
    [HttpGet(Name = "SolarWatch/{cityName}/{date}")]
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

            Coordinates coordinates;
            try
            {
                var dbCity = _cityService.GetByName(cityName);

                if (dbCity != null)
                {
                    coordinates = new Coordinates { Lat = dbCity.Lat, Lon = dbCity.Lon };
                    _logger.LogInformation("Coordinates set from DB");
                }
                else
                {
                    _logger.LogInformation($"DB does not contain city with this name: {cityName}");
                    string openWeatherData;
                    try
                    {
                        openWeatherData = await _cityService.GetOpenWeatherMapApiDataAsync(cityName);
                        coordinates = _cityService.ProcessCityCoordinates(openWeatherData);
                    
                        _logger.LogInformation("Coordinates data fetched from external API");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                        return BadRequest("");
                    }

                    try
                    {
                        var newCity = _cityService.ProcessCity(openWeatherData);
                        _cityService.AddCityToDb(newCity);
                        _logger.LogInformation($"City: {cityName} added to DB");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                        return BadRequest($"Failed to add city {cityName} to DB: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            string sunriseSunsetData;
            try
            {
                sunriseSunsetData = await _sunriseSunsetService.GetSunriseSunsetApiDataAsync(date, coordinates);
                _logger.LogInformation("SunriseSunset data fetched from external API");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return NotFound($"Not found sunriseSunset with these coordinates: {coordinates.Lat}, {coordinates.Lon}, date: {date}");
            }
            
            try
            {
                var newSunriseSunset = _sunriseSunsetService.ProcessSunriseSunset(sunriseSunsetData);
                newSunriseSunset.CityName = cityName;
                _sunriseSunsetService.AddSunriseSunsetToDb(newSunriseSunset);
                _logger.LogInformation($"SunriseSunset with date: {date} added to {cityName} in DB");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest($"Failed to add sunrise/sunset data to DB: {e.Message}");
            }
            
            var solarWatch = _sunriseSunsetService.ProcessSolarWatch(sunriseSunsetData);
            solarWatch.City = cityName;
            return Ok(solarWatch);
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return NotFound($"Failed to get sunrise/sunset data from DB: {e.Message}");
        }
    }
}