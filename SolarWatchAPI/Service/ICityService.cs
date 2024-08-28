using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ICityService
{
    IEnumerable<City> GetAll();
    City? GetByName(string cityName);
    void AddCityToDb(City city);
    void DeleteCityFromDb(string cityName);
    void UpdateCityInDb(City city);
    Task<string> GetOpenWeatherMapApiDataAsync(string cityName);
    Coordinates ProcessCityCoordinates(string jsonData);
    City ProcessCity(string jsonData);
}