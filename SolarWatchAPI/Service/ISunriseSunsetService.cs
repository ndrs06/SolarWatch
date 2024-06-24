using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ISunriseSunsetService
{
    SunriseSunset? GetByCityIdAndDate(string cityName, DateTime date);
    void AddSunriseSunsetToDb(SunriseSunset sunriseSunset);
    Task<string> GetSunriseSunsetApiDataAsync(DateTime date, Coordinates coordinates);
    SolarWatch ProcessSolarWatch(string jsonData);
    SunriseSunset ProcessSunriseSunset(string jsonData);
}