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
    private string? SunriseSunsetJsonData  { get; set; }

    public SunriseSunsetService(ILogger<SunriseSunsetService> logger, ISunriseSunsetRepository sunriseSunsetRepository, ISunriseSunsetApiDataProvider sunriseSunsetApiDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _sunriseSunsetRepository = sunriseSunsetRepository;
        _sunriseSunsetApiDataProvider = sunriseSunsetApiDataProvider;
        _jsonProcessor = jsonProcessor;
    }

    public SunriseSunset? GetByCityIdAndDate(string cityName, DateTime date)
    {
        return _sunriseSunsetRepository.GetByCityAndDate(cityName, date);
    }
    
    public void AddSunriseSunsetToDb(string cityName)
    {
        var sunriseSunset = _jsonProcessor.ProcessSunriseSunset(SunriseSunsetJsonData);
        sunriseSunset.CityName = cityName;
        
        _sunriseSunsetRepository.Add(sunriseSunset);
    }

    public async Task<SolarWatch> GetSolarWatchAsync(DateTime date, Coordinates coordinates)
    {
        SunriseSunsetJsonData = await _sunriseSunsetApiDataProvider.GetAsync(date, coordinates);

        return _jsonProcessor.ProcessSolarWatch(SunriseSunsetJsonData);
    }
}