using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service.JsonProcessors;

public interface IJsonProcessor
{
    Coordinates Process(string data);
}