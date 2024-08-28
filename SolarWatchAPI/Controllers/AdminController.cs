using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Model.RequestModels;
using SolarWatchAPI.Service;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly ICityService _cityService;
    private readonly ISunriseSunsetService _sunriseSunsetService;

    public AdminController(ILogger<AdminController> logger, ICityService cityService, ISunriseSunsetService sunriseSunsetService)
    {
        _logger = logger;
        _cityService = cityService;
        _sunriseSunsetService = sunriseSunsetService;
    }
    
    [HttpGet("cities")]
    public ActionResult<IEnumerable<City>> GetAllCities()
    {
        try
        {
            var cities = _cityService.GetAll();
            return Ok(cities);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("cities/{cityName}")]
    public ActionResult<City> GetCityByName(string cityName)
    {
        try
        {
            var city = _cityService.GetByName(cityName);
            return Ok(city);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("cities")]
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
    
    [HttpDelete("cities")]
    public ActionResult<string> DeleteCityFromDb(string cityName)
    {
        try
        {
            _cityService.DeleteCityFromDb(cityName);
            return Ok($"{cityName} deleted from DB");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("cities/{cityName}")]
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