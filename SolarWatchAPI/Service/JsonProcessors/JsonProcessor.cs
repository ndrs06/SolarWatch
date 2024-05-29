using System.Text.Json;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Service.JsonProcessors;

public class JsonProcessor : IJsonProcessor
{
    public Coordinates ProcessCoordinates(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

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

    private DateTime GetDateTimeFromString(string date)
    {
        var dateTime = DateTime.Parse(date[..10]);

        return dateTime;
    }

    private TimeOnly GetTimeOnlyFromString(string date)
    {
        var time = DateTimeOffset.Parse(date);
        var timeOnly = new TimeOnly(time.TimeOfDay.Hours, time.TimeOfDay.Minutes);

        return timeOnly;
    }
}