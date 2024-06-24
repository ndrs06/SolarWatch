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

    public void AddCityToDb(City city)
    {
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

    public async Task<string> GetOpenWeatherMapApiDataAsync(string cityName)
    {
        return await _openWeatherMapApiDataProvider.GetAsync(cityName);
    }

    public Coordinates ProcessCityCoordinates(string jsonData)
    {
        return _jsonProcessor.ProcessCoordinates(jsonData);
    }

    public City ProcessCity(string jsonData)
    {
        return _jsonProcessor.ProcessCity(jsonData);
    }
}