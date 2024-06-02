using SolarWatchAPI.Model;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;

namespace SolarWatchAPI.Service;

public class CoordinatesService : ICoordinatesService
{
    private readonly ILogger<CoordinatesService> _logger;
    private readonly ICoordinatesProvider _coordinatesProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public CoordinatesService(ILogger<CoordinatesService> logger, ICoordinatesProvider coordinatesProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _coordinatesProvider = coordinatesProvider;
        _jsonProcessor = jsonProcessor;
    }

    public async Task<Coordinates> GetAsync(string cityName)
    {
        var coordinatesData = await _coordinatesProvider.GetAsync(cityName);
        return _jsonProcessor.ProcessCoordinates(coordinatesData);
    }
}