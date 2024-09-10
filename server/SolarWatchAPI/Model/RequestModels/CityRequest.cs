namespace SolarWatchAPI.Model.RequestModels;

public record CityRequest(string Name, double Lat, double Lon, string State, string Country);