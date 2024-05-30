namespace SolarWatchAPI.Service.DataProviders;

public interface ICoordinatesProvider
{
    Task<string> GetCoordinates(string city);
}