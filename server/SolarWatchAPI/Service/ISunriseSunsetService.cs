using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ISunriseSunsetService
{
    SunriseSunset? GetByCityNameAndDate(string cityName, DateTime date);
    Task AddSunriseSunsetToDb(string cityName, DateTime date);
    void DeleteSunriseSunsetFromDb(string cityName, DateTime date);
}