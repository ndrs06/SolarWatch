using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service.DataProviders;

public interface ISunriseSunsetApiDataProvider
{
    Task<string> GetAsync(DateTime date, Coordinates coordinates);
}