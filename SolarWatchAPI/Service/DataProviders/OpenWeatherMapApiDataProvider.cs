using System.Net;
using dotenv.net;

namespace SolarWatchAPI.Service.DataProviders;

public class OpenWeatherMapApiDataProvider : IOpenWeatherMapApiDataProvider
{
    private readonly ILogger<OpenWeatherMapApiDataProvider> _logger;
    private readonly IConfiguration _configuration;

    public OpenWeatherMapApiDataProvider(ILogger<OpenWeatherMapApiDataProvider> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task<string> GetAsync(string cityName)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={_configuration.GetConnectionString("OPEN_WEATHER_API_KEY")}";
        
        using var client = new HttpClient();
        var res = await client.GetAsync(url);
        
        _logger.LogInformation("");

        return await res.Content.ReadAsStringAsync();
    }
}
