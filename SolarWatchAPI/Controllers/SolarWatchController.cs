using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;
using SolarWatchAPI.Service.Repository;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICoordinatesProvider _coordinatesProvider;
    private readonly ISolarWatchDataProvider _solarWatchDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICoordinatesProvider coordinatesProvider, ISolarWatchDataProvider solarWatchDataProvider, IJsonProcessor jsonProcessor, ICityRepository cityRepository, ISunriseSunsetRepository sunriseSunsetRepository)
    {
        _logger = logger;
        _coordinatesProvider = coordinatesProvider;
        _solarWatchDataProvider = solarWatchDataProvider;
        _jsonProcessor = jsonProcessor;
        _cityRepository = cityRepository;
        _sunriseSunsetRepository = sunriseSunsetRepository;
    }
    
    // DbContext
    [HttpGet(Name = "SolarWatch")]
    public async Task<ActionResult<SolarWatch>> Get(string city, DateTime date)
    {
        Coordinates coordinates;
        var dbCity = _cityRepository.GetByName(city);

        if (dbCity == null)
        {
            _logger.LogInformation($"DB does not contains city with this name: {city}");
            try
            {
                var coordinatesData = await _coordinatesProvider.GetCoordinates(city);
                coordinates = _jsonProcessor.ProcessCoordinates(coordinatesData);
                
                var newCity = new City
                {
                    Name = city,
                    Lat = coordinates.Lat,
                    Lon = coordinates.Lon
                };
                _cityRepository.Add(newCity);
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

                var newSunriseSunset = new SunriseSunset
                {
                    Date = solarWatch.Date,
                    Sunrise = solarWatch.Sunrise,
                    Sunset = solarWatch.Sunset,
                    CityId = _cityRepository.GetByName(solarWatch.City).Id
                };
                _sunriseSunsetRepository.Add(newSunriseSunset);
                
                return Ok(solarWatch);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound("Error getting data");
            }
        }
        
        coordinates = new Coordinates{Lat = dbCity.Lat, Lon = dbCity.Lon};
        var dbSunriseSunset = _sunriseSunsetRepository.GetByCityAndDate(dbCity.Id, date);
            
        if (dbSunriseSunset == null)
        {
            try
            {
                var solarWatchData = await _solarWatchDataProvider.GetCurrent(date, coordinates.Lat, coordinates.Lon);
                var solarWatch = _jsonProcessor.ProcessSolarWatch(solarWatchData);
                solarWatch.City = city;

                var newSunriseSunset = new SunriseSunset
                {
                    Date = solarWatch.Date,
                    Sunrise = solarWatch.Sunrise,
                    Sunset = solarWatch.Sunset,
                    CityId = _cityRepository.GetByName(solarWatch.City).Id,
                    City = _cityRepository.GetByName(solarWatch.City)
                };
                _sunriseSunsetRepository.Add(newSunriseSunset);
                
                return Ok(solarWatch);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound("Error getting data");
            }
        }
            
        return Ok(new SolarWatch
        {
            City = dbCity.Name,
            Date = dbSunriseSunset.Date,
            Sunrise = dbSunriseSunset.Sunrise,
            Sunset = dbSunriseSunset.Sunset
        });
        
    }
    
    /*[HttpGet(Name = "SolarWatch")]
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
    }*/
}
