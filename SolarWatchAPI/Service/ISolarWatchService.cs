using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service;

public interface ISolarWatchService
{
    Task<SolarWatch> GetAsync(DateTime date, Coordinates coordinates);
}