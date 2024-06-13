using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ICityService
{
    City? GetByName(string cityName);
    void AddCityToDb(City? city = null);
    void DeleteCityFromDb(City city);
    void UpdateCityInDb(City city);
    Task<Coordinates> GetCityCoordinatesAsync(string cityName);
    Task<City> GetCityAsync(string cityName);
}