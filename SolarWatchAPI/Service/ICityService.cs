using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ICityService
{
    City? GetByName(string cityName);
    void Add(City city);
}