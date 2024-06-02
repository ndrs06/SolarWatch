namespace SolarWatchAPI.Service.DataProviders;

public interface ICoordinatesProvider
{
    Task<string> GetAsync(string cityName);
}