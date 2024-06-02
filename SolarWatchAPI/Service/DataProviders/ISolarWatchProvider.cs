namespace SolarWatchAPI.Service.DataProviders;

public interface ISolarWatchProvider
{
    Task<string> GetAsync(DateTime date, double lat, double lon);
}