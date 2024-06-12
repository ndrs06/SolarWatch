using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ISunriseSunsetService
{
    SunriseSunset? GetByCityIdAndDate(string cityName, DateTime date);
    void AddSunriseSunsetToDb(string cityName);
    Task<SolarWatch> GetSolarWatchAsync(DateTime date, Coordinates coordinates);
}