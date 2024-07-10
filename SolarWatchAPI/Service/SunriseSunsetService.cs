using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;
using SolarWatchAPI.Service.Repository;

namespace SolarWatchAPI.Service;

public class SunriseSunsetService : ISunriseSunsetService
{
    private readonly ILogger<SunriseSunsetService> _logger;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;
    private readonly ISunriseSunsetApiDataProvider _sunriseSunsetApiDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SunriseSunsetService(ILogger<SunriseSunsetService> logger, ISunriseSunsetRepository sunriseSunsetRepository, ISunriseSunsetApiDataProvider sunriseSunsetApiDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _sunriseSunsetRepository = sunriseSunsetRepository;
        _sunriseSunsetApiDataProvider = sunriseSunsetApiDataProvider;
        _jsonProcessor = jsonProcessor;
    }

    public SunriseSunset? GetByCityNameAndDate(string cityName, DateTime date)
    {
        return _sunriseSunsetRepository.GetByCityNameAndDate(cityName, date);
    }
    
    public void AddSunriseSunsetToDb(SunriseSunset sunriseSunset)
    {
        _sunriseSunsetRepository.Add(sunriseSunset);
    }

    public async Task<string> GetSunriseSunsetApiDataAsync(DateTime date, Coordinates coordinates)
    {
        return await _sunriseSunsetApiDataProvider.GetAsync(date, coordinates);
    }

    public SolarWatch ProcessSolarWatch(string jsonData)
    {
        return _jsonProcessor.ProcessSolarWatch(jsonData);
    }

    public SunriseSunset ProcessSunriseSunset(string jsonData)
    {
        return _jsonProcessor.ProcessSunriseSunset(jsonData);
    }
}