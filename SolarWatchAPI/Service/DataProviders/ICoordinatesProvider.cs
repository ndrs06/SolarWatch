namespace SolarWatchAPI.Service.DataProviders;

public interface ICoordinatesProvider
{
    string GetCoordinates(string city);
}