using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;
using SolarWatchAPI.Service.Repository;

namespace SolarWatchAPI.Service;

public class SunriseSunsetService : ISunriseSunsetService
{
    private readonly ILogger<SunriseSunsetService> _logger;
    private readonly ICityRepository _cityRepository;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;
    private readonly ISunriseSunsetApiDataProvider _sunriseSunsetApiDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SunriseSunsetService(ILogger<SunriseSunsetService> logger, ISunriseSunsetRepository sunriseSunsetRepository, ISunriseSunsetApiDataProvider sunriseSunsetApiDataProvider, IJsonProcessor jsonProcessor, ICityRepository cityRepository)
    {
        _logger = logger;
        _sunriseSunsetRepository = sunriseSunsetRepository;
        _sunriseSunsetApiDataProvider = sunriseSunsetApiDataProvider;
        _jsonProcessor = jsonProcessor;
        _cityRepository = cityRepository;
    }

    public SunriseSunset? GetByCityNameAndDate(string cityName, DateTime date)
    {
        return _sunriseSunsetRepository.GetByCityNameAndDate(cityName, date);
    }
    
    public async Task AddSunriseSunsetToDb(string cityName, DateTime date)
    {
        var sunriseSunset = _sunriseSunsetRepository.GetByCityNameAndDate(cityName, date);
        if (sunriseSunset != null)
        {
            throw new Exception($"Sunrise sunset already exists: {sunriseSunset}");
        }
        
        var city = _cityRepository.GetByName(cityName);
        if (city == null)
        {
            throw new Exception($"City not found: {cityName}");
        }
        var coordinates = new Coordinates {Lat = city.Lat, Lon = city.Lon};
        var jsonData = await _sunriseSunsetApiDataProvider.GetAsync(date, coordinates);
        sunriseSunset = _jsonProcessor.ProcessSunriseSunset(jsonData);
        sunriseSunset.CityName = city.Name;
        _sunriseSunsetRepository.Add(sunriseSunset);
    }

    public void DeleteSunriseSunsetFromDb(string cityName, DateTime date)
    {
        var sunriseSunset = _sunriseSunsetRepository.GetByCityNameAndDate(cityName, date);

        if (sunriseSunset == null)
        {
            throw new Exception($"Sunrise sunset not found: {sunriseSunset}");
        }
        
        _sunriseSunsetRepository.Delete(sunriseSunset);
    }
}