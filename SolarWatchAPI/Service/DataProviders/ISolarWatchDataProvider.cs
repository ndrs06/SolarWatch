namespace SolarWatchAPI.Service.DataProviders;

public interface ISolarWatchDataProvider
{
    Task<string> GetCurrent(DateTime date, double lat, double lon);
}