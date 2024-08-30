namespace SolarWatchAPI.Model.RequestModels;

public record SunriseSunsetRequest(DateTime Date, TimeOnly Sunrise, TimeOnly Sunset, string CityName);