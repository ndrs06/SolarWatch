using System.Net;
using dotenv.net;

namespace SolarWatchAPI.Service.DataProviders;

public class CoordinatesProvider : ICoordinatesProvider
{
    private readonly ILogger<CoordinatesProvider> _logger;
    private readonly string _openWeatherKey;

    public CoordinatesProvider(ILogger<CoordinatesProvider> logger)
    {
        _logger = logger;
        DotEnv.Load();
        var envVars = DotEnv.Read();
        _openWeatherKey = envVars["OPEN_WEATHER_API_KEY"];
    }
    
    public async Task<string> GetAsync(string cityName)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={_openWeatherKey}";

        using var client = new HttpClient();
        var res = await client.GetAsync(url);

        return await res.Content.ReadAsStringAsync();
    }
}
