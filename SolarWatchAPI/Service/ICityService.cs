using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Model.RequestModels;

namespace SolarWatchAPI.Service;

public interface ICityService
{
    IEnumerable<City> GetAll();
    City? GetByName(string cityName);
    Task AddCityToDb(string cityName);
    void DeleteCityFromDb(string cityName);
    void UpdateCityInDb(string cityName, CityRequest request);
}