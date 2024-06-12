using SolarWatchAPI.Model;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;

namespace SolarWatchAPI.Service;

public class SolarWatchService : ISolarWatchService
{
    private readonly ILogger<SolarWatchService> _logger;
    private readonly ISunriseSunsetApiDataProvider _sunriseSunsetApiDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SolarWatchService(ILogger<SolarWatchService> logger, ISunriseSunsetApiDataProvider sunriseSunsetApiDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _sunriseSunsetApiDataProvider = sunriseSunsetApiDataProvider;
        _jsonProcessor = jsonProcessor;
    }

    public async Task<SolarWatch> GetAsync(DateTime date, Coordinates coordinates)
    {
        var solarWatchData = await _sunriseSunsetApiDataProvider.GetAsync(date, coordinates.Lat, coordinates.Lon);
        return _jsonProcessor.ProcessSolarWatch(solarWatchData);
    }
}