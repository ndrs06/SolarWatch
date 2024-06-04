using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;
using SolarWatchAPI.Service.Repository;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICityService _cityService;
    private readonly ISunriseSunsetService _sunriseSunsetService;
    private readonly ICoordinatesService _coordinatesService;
    private readonly ISolarWatchService _solarWatchService;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICityService cityService, ISunriseSunsetService sunriseSunsetService, ICoordinatesService coordinatesService, ISolarWatchService solarWatchService)
    {
        _logger = logger;
        _cityService = cityService;
        _sunriseSunsetService = sunriseSunsetService;
        _coordinatesService = coordinatesService;
        _solarWatchService = solarWatchService;
    }

    [HttpGet(Name = "SolarWatch")]
    public async Task<ActionResult<SolarWatch>> Get(string cityName, DateTime date)
    {
        try
        {
            Coordinates coordinates;
            var dbCity = _cityService.GetByName(cityName);

            if (dbCity == null)
            {
                _logger.LogInformation($"DB does not contains city with this name: {cityName}");
                try
                {
                    coordinates = await _coordinatesService.GetAsync(cityName);

                    var newCity = new City
                    {
                        Name = cityName,
                        Lat = coordinates.Lat,
                        Lon = coordinates.Lon,
                        State = "HU",
                        Coutry = "HU"
                    };
                    _cityService.Add(newCity);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, ":)");
                    return NotFound($"Not found city with this name: {cityName}");
                }
            }
            else
            {
                coordinates = new Coordinates { Lat = dbCity.Lat, Lon = dbCity.Lon };
            }

            var dbSunriseSunset = _sunriseSunsetService.GetByCityIdAndDate(dbCity.Id, date);

            if (dbSunriseSunset == null)
            {
                try
                {
                    var solarWatch = await _solarWatchService.GetAsync(date, coordinates);
                    solarWatch.City = cityName;

                    var newSunriseSunset = new SunriseSunset
                    {
                        Date = solarWatch.Date,
                        Sunrise = solarWatch.Sunrise,
                        Sunset = solarWatch.Sunset,
                        CityId = _cityService.GetByName(solarWatch.City).Id,
                        City = _cityService.GetByName(solarWatch.City)
                    };
                    _sunriseSunsetService.Add(newSunriseSunset);

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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    
    // DbContext
    /*[HttpGet(Name = "SolarWatch")]
    public async Task<ActionResult<SolarWatch>> Get(string cityName, DateTime date)
    {
        Coordinates coordinates;
        var dbCity = _cityService.GetByName(cityName);

        if (dbCity == null)
        {
            _logger.LogInformation($"DB does not contains city with this name: {cityName}");
            try
            {
                coordinates = await _coordinatesService.GetAsync(cityName);
                
                var newCity = new City
                {
                    Name = cityName,
                    Lat = coordinates.Lat,
                    Lon = coordinates.Lon
                };
                _cityService.Add(newCity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound($"Not found city with this name: {cityName}");
            }
            
            try
            {
                var solarWatch = await _solarWatchService.GetAsync(date, coordinates);
                solarWatch.City = cityName;

                var newSunriseSunset = new SunriseSunset
                {
                    Date = solarWatch.Date,
                    Sunrise = solarWatch.Sunrise,
                    Sunset = solarWatch.Sunset,
                    CityId = _cityService.GetByName(solarWatch.City).Id
                };
                _sunriseSunsetService.Add(newSunriseSunset);
                
                return Ok(solarWatch);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound("Error getting data");
            }
        }
        
        coordinates = new Coordinates{Lat = dbCity.Lat, Lon = dbCity.Lon};
        var dbSunriseSunset = _sunriseSunsetService.GetByCityIdAndDate(dbCity.Id, date);
            
        if (dbSunriseSunset == null)
        {
            try
            {
                var solarWatch = await _solarWatchService.GetAsync(date, coordinates);
                solarWatch.City = cityName;

                var newSunriseSunset = new SunriseSunset
                {
                    Date = solarWatch.Date,
                    Sunrise = solarWatch.Sunrise,
                    Sunset = solarWatch.Sunset,
                    CityId = _cityService.GetByName(solarWatch.City).Id,
                    City = _cityService.GetByName(solarWatch.City)
                };
                _sunriseSunsetService.Add(newSunriseSunset);
                
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
        
    }*/
    
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

