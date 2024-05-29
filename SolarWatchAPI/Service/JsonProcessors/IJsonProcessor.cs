using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service.JsonProcessors;

public interface IJsonProcessor
{
    Coordinates ProcessCoordinates(string data);
    SolarWatch ProcessSolarWatch(string data);
}