namespace SolarWatchAPI.Service.DataProviders;

public interface ISunriseSunsetApiDataProvider
{
    Task<string> GetAsync(DateTime date, double lat, double lon);
}