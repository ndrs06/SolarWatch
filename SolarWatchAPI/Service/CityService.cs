using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;
using SolarWatchAPI.Service.Repository;

namespace SolarWatchAPI.Service;

public class CityService : ICityService
{
    private readonly ILogger<CityService> _logger;
    private readonly ICityRepository _cityRepository;
    private readonly IOpenWeatherMapApiDataProvider _openWeatherMapApiDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private string? OpenWeatherJsonData { get; set; }

    public CityService(ILogger<CityService> logger, ICityRepository cityRepository, IJsonProcessor jsonProcessor, IOpenWeatherMapApiDataProvider openWeatherMapApiDataProvider)
    {
        _logger = logger;
        _cityRepository = cityRepository;
        _jsonProcessor = jsonProcessor;
        _openWeatherMapApiDataProvider = openWeatherMapApiDataProvider;
    }

    public City? GetByName(string? cityName)
    {
        return _cityRepository.GetByName(cityName);
    }

    public void AddCityToDb(City? city = null)
    {
        if (city == null)
        {
            city = _jsonProcessor.ProcessCity(OpenWeatherJsonData);
        }
        
        _cityRepository.Add(city);
    }

    public void DeleteCityFromDb(City city)
    {
        _cityRepository.Delete(city);
    }

    public void UpdateCityInDb(City city)
    {
        _cityRepository.Update(city);
    }

    public async Task<Coordinates> GetCityCoordinatesAsync(string? cityName)
    {
        OpenWeatherJsonData = await _openWeatherMapApiDataProvider.GetAsync(cityName);
        
        return _jsonProcessor.ProcessCoordinates(OpenWeatherJsonData);
    }

    public async Task<City> GetCityAsync(string cityName)
    {
        OpenWeatherJsonData = await _openWeatherMapApiDataProvider.GetAsync(cityName);
        
        return _jsonProcessor.ProcessCity(OpenWeatherJsonData);
    }
}