using SolarWatchAPI.Model;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;

namespace SolarWatchAPI.Service;

public class CoordinatesService : ICoordinatesService
{
    private readonly ILogger<CoordinatesService> _logger;
    private readonly IOpenWeatherMapApiDataProvider _openWeatherMapApiDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public CoordinatesService(ILogger<CoordinatesService> logger, IOpenWeatherMapApiDataProvider openWeatherMapApiDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _openWeatherMapApiDataProvider = openWeatherMapApiDataProvider;
        _jsonProcessor = jsonProcessor;
    }

    public async Task<Coordinates> GetAsync(string cityName)
    {
        var coordinatesData = await _openWeatherMapApiDataProvider.GetAsync(cityName);
        return _jsonProcessor.ProcessCoordinates(coordinatesData);
    }
}