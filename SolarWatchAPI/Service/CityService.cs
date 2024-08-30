using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Model.RequestModels;
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

    public IEnumerable<City> GetAll()
    {
        return _cityRepository.GetAll();
    }

    public City? GetByName(string? cityName)
    {
        return _cityRepository.GetByName(cityName);
    }

    public async void AddCityToDb(string cityName)
    {
        var city = _cityRepository.GetByName(cityName);

        if (city != null)
        {
            throw new Exception($"City {cityName} already exists");
        }
        
        var jsonData = await _openWeatherMapApiDataProvider.GetAsync(cityName);
        var newCity = _jsonProcessor.ProcessCity(jsonData);
        _cityRepository.Add(newCity);
    }

    public void DeleteCityFromDb(string cityName)
    {
        var city = _cityRepository.GetByName(cityName);
        
        if (city == null)
        {
            throw new Exception("City not found");
        }
        
        _cityRepository.Delete(city);
    }

    public void UpdateCityInDb(string cityName, CityRequest request)
    {
        var city = _cityRepository.GetByName(cityName);
        
        if (city == null)
        {
            throw new Exception("City not found");
        }
        
        city.Country = request.Country;
        city.State = request.State;
        city.Lon = request.Lon;
        city.Lat = request.Lat;
        
        _cityRepository.Update(city);
    }
}