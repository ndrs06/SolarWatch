using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service;

public interface ICoordinatesService
{
    Task<Coordinates> GetAsync(string cityName);
}