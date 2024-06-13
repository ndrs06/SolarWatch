using SolarWatchAPI.Model;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service.JsonProcessors;

public interface IJsonProcessor
{
    Coordinates ProcessCoordinates(string data);
    SolarWatch ProcessSolarWatch(string data);
    City ProcessCity(string data);
    SunriseSunset ProcessSunriseSunset(string data);
}