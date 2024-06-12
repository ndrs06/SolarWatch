namespace SolarWatchAPI.Service.DataProviders;

public interface IOpenWeatherMapApiDataProvider
{
    Task<string> GetAsync(string cityName);
}