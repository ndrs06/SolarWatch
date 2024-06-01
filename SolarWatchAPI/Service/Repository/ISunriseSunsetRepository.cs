using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service.Repository;

public interface ISunriseSunsetRepository
{
    IEnumerable<SunriseSunset> GetAll();
    SunriseSunset? GetByName(string name);
    void Add(SunriseSunset sunriseSunset);
    void Delete(SunriseSunset sunriseSunset);
    void Update(SunriseSunset sunriseSunset);
}