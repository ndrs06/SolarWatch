using System.Text.Json;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service.JsonProcessors;

public class JsonProcessor : IJsonProcessor
{
    public Coordinates Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        Coordinates coordinates = new Coordinates
        { 
            Lat = json.RootElement.GetProperty("lat").GetDouble(),
            Lon = json.RootElement.GetProperty("lon").GetDouble()
        };

        return coordinates;
    }
}