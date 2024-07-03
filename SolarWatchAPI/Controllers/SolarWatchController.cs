using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Model.RequestModels;
using SolarWatchAPI.Service;
using SolarWatchAPI.Service.DataProviders;

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
    public async Task<ActionResult<SolarWatch>> Get(string cityName, DateTime date)
    {
        string openWeatherMapJsonData;
        string sunriseSunsetJsonData;
        City? dbCity;
        SunriseSunset? dbSunriseSunset;
        Coordinates coordinates;
        SolarWatch solarWatch;

        try
        {
            dbCity = _cityService.GetByName(cityName);

            if (dbCity != null)
            {
                _logger.LogInformation("City data set from DB");
                coordinates = new Coordinates { Lat = dbCity.Lat, Lon = dbCity.Lon };
            }
            else
            {
                _logger.LogInformation($"DB does not contain city with this name: {cityName}");
                try
                {
                    openWeatherMapJsonData = await _cityService.GetOpenWeatherMapApiDataAsync(cityName);
                    coordinates = _cityService.ProcessCityCoordinates(openWeatherMapJsonData);
                    
                    _logger.LogInformation("Coordinates data fetched from external API");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return NotFound("");
                }

                try
                {
                    var newCity = _cityService.ProcessCity(openWeatherMapJsonData);
                    _cityService.AddCityToDb(newCity);
                    _logger.LogInformation($"City: {cityName} added to DB");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return BadRequest($"Failed to add city {cityName} to DB: {e.Message}");
                }
            }

            try
            {
                dbSunriseSunset = _sunriseSunsetService.GetByCityIdAndDate(cityName, date);
                if (dbSunriseSunset != null)
                {
                    _logger.LogInformation("SunriseSunset data set from DB");
                    return Ok(new SolarWatch
                    {
                        City = cityName,
                        Date = dbSunriseSunset.Date,
                        Sunrise = dbSunriseSunset.Sunrise,
                        Sunset = dbSunriseSunset.Sunset
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return NotFound($"Failed to get sunrise/sunset data from DB: {e.Message}");
            }

            try
            {
                sunriseSunsetJsonData = await _sunriseSunsetService.GetSunriseSunsetApiDataAsync(date, coordinates);
                solarWatch = _sunriseSunsetService.ProcessSolarWatch(sunriseSunsetJsonData);
                solarWatch.City = cityName;
                
                _logger.LogInformation("SunriseSunset data fetched from external API");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return NotFound($"Not found sunriseSunset with these coordinates: {coordinates.Lat}, {coordinates.Lon}, date: {date}");
            }

            try
            {
                var newSunriseSunset = _sunriseSunsetService.ProcessSunriseSunset(sunriseSunsetJsonData);
                newSunriseSunset.CityName = cityName;
                _sunriseSunsetService.AddSunriseSunsetToDb(newSunriseSunset);
                _logger.LogInformation($"SunriseSunset with date: {date} added to {cityName} in DB");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest($"Failed to add sunrise/sunset data to DB: {e.Message}");
            }

            return Ok(solarWatch);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPost("addNewCityToDb"), Authorize(Roles="Admin")]
    public async Task<ActionResult<string>> PostCityToDb(string cityName)
    {
        try
        {
            var city = _cityService.GetByName(cityName);

            if (city == null)
            {
                var jsonData = await _cityService.GetOpenWeatherMapApiDataAsync(cityName);
                var newCity = _cityService.ProcessCity(jsonData);
                _cityService.AddCityToDb(newCity);

                return Ok($"{cityName} added to DB");
            }

            return BadRequest($"{cityName} already exist in DB");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("deleteCityFromDb"), Authorize(Roles="Admin")]
    public ActionResult<string> DeleteCityFromDb(string cityName)
    {
        try
        {
            var city = _cityService.GetByName(cityName);

            if (city != null)
            {
                _cityService.DeleteCityFromDb(city);

                return Ok($"{cityName} deleted from DB");
            }

            return NotFound($"{cityName} does not exist in DB");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("addNewCityToDb/{cityName}"), Authorize(Roles="Admin")]
    public ActionResult<string> UpdateCityInDb(string cityName, [FromBody] CityRequest request)
    {
        try
        {
            var city = _cityService.GetByName(cityName);

            if (city != null)
            {
                var updatedCity = new City
                {
                    Name = cityName,
                    Lat = (int)request.Lat == 0 ? city.Lat : request.Lon,
                    Lon = (int)request.Lon == 0 ? city.Lon : request.Lon,
                    State = request.State == "string" ? city.State : request.State,
                    Country = request.Country == "string" ? city.Country : request.Country
                };
                _cityService.UpdateCityInDb(updatedCity);

                return Ok($"{cityName} updated in DB");
            }

            return NotFound($"{cityName} does not exist in DB");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}