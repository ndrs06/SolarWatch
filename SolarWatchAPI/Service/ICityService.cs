using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ICityService
{
    City? GetByName(string cityName);
    void AddCityToDb();
    Task<Coordinates> GetCityCoordinatesAsync(string cityName);
}