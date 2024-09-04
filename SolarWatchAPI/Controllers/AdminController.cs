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

    #region Cities
    [HttpGet("cities")]
    public ActionResult<IEnumerable<City>> GetAllCities()
    {
        try
        {
            return Ok(_cityService.GetAll());
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
            return Ok(_cityService.GetByName(cityName));
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
            await _cityService.AddCityToDb(cityName);
            return Ok($"City {cityName} was successfully added.");
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
    
    [HttpPatch("cities/{cityName}")]
    public ActionResult<string> UpdateCityInDb(string cityName, [FromBody] CityRequest request)
    {
        try
        {
            _cityService.UpdateCityInDb(cityName, request);
            return Ok($"{cityName} updated in DB");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    #endregion

    #region SunriseSunsets

    [HttpPost("sunrise-sunsets")]
    public async Task<ActionResult<string>> PostSunriseSunsetToDb(string cityName, DateTime date)
    {
        try
        {
            await _sunriseSunsetService.AddSunriseSunsetToDb(cityName, date);
            return Ok($"Sunrise sunset to {cityName} was successfully added.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("sunrise-sunsets")]
    public ActionResult<string> DeleteSunriseSunsetFromDb(string cityName, DateTime date)
    {
        try
        {
            _sunriseSunsetService.DeleteSunriseSunsetFromDb(cityName, date);
            return Ok($"Sunrise sunset from {cityName} was successfully deleted.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }

    #endregion
}