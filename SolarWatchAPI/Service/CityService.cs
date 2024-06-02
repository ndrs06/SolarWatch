using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service.Repository;

namespace SolarWatchAPI.Service;

public class CityService : ICityService
{
    private readonly ILogger<CityService> _logger;
    private readonly ICityRepository _cityRepository;

    public CityService(ILogger<CityService> logger, ICityRepository cityRepository)
    {
        _logger = logger;
        _cityRepository = cityRepository;
    }

    public City? GetByName(string cityName)
    {
        return _cityRepository.GetByName(cityName);
    }

    public void Add(City city)
    {
        _cityRepository.Add(city);
    }
}