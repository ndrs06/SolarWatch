using System.Text.Json;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service.JsonProcessors;

public class JsonProcessor : IJsonProcessor
{
    private readonly ILogger<JsonProcessor> _logger;

    public JsonProcessor(ILogger<JsonProcessor> logger)
    {
        _logger = logger;
    }

    public Coordinates ProcessCoordinates(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        _logger.LogInformation("Coordinates JSON data parsed");

        Coordinates coordinates = new Coordinates
        { 
            Lat = json.RootElement[0].GetProperty("lat").GetDouble(),
            Lon = json.RootElement[0].GetProperty("lon").GetDouble()
        };

        return coordinates;
    }

    public SolarWatch ProcessSolarWatch(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement result = json.RootElement.GetProperty("results");

        var solarWatch = new SolarWatch
        {
            Date = GetDateTimeFromString(result.GetProperty("sunrise").GetString()),
            Sunrise = GetTimeOnlyFromString(result.GetProperty("sunrise").GetString()),
            Sunset = GetTimeOnlyFromString(result.GetProperty("sunset").GetString()),
        };

        return solarWatch;
    }

    private DateTime GetDateTimeFromString(string dateStr)
    {
        var dateTime = DateTime.Parse(dateStr[..10]);

        return dateTime;
    }

    private TimeOnly GetTimeOnlyFromString(string dateStr)
    {
        var time = DateTimeOffset.Parse(dateStr);
        var timeOnly = new TimeOnly(time.TimeOfDay.Hours, time.TimeOfDay.Minutes);

        return timeOnly;
    }
}