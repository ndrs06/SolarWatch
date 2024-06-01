using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service.Repository;

public interface ISunriseSunsetRepository
{
    IEnumerable<SunriseSunset> GetAll();
    SunriseSunset? GetByCityAndDate(int cityId, DateTime date);
    void Add(SunriseSunset sunriseSunset);
    void Delete(SunriseSunset sunriseSunset);
    void Update(SunriseSunset sunriseSunset);
}