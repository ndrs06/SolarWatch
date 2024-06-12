using System.Net;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service.DataProviders;

public class SunriseSunsetApiDataProvider : ISunriseSunsetApiDataProvider
{
    private readonly ILogger<SunriseSunsetApiDataProvider> _logger;

    public SunriseSunsetApiDataProvider(ILogger<SunriseSunsetApiDataProvider> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetAsync(DateTime date, Coordinates coordinates)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={coordinates.Lat}&lng={coordinates.Lon}&date={date}&formatted=0";

        using var client = new HttpClient();
        var res = await client.GetAsync(url);

        return await res.Content.ReadAsStringAsync();
    }
}