namespace SolarWatchAPI.Service.DataProviders;

public interface ISolarWatchDataProvider
{
    string GetCurrent(DateTime date, double lat, double lon);
}