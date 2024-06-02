using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service;

public interface ISunriseSunsetService
{
    SunriseSunset? GetByCityIdAndDate(int cityId, DateTime date);
    void Add(SunriseSunset sunriseSunset);
}