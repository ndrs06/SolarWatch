using System.Net;

namespace SolarWatchAPI.Service.DataProviders;

public class SolarWatchDataProvider : ISolarWatchDataProvider
{
    private readonly ILogger<SolarWatchDataProvider> _logger;

    public SolarWatchDataProvider(ILogger<SolarWatchDataProvider> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetCurrent(DateTime date, double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}&formatted=0";

        using var client = new HttpClient();
        var res = await client.GetAsync(url);

        return await res.Content.ReadAsStringAsync();
    }
}