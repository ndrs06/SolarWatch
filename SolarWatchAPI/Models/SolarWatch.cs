namespace SolarWatchAPI.Models;

public class SolarWatch
{
    public DateTime Date { get; set; }
    public string City { get; set; }
    public TimeOnly Sunset { get; set; }
    public TimeOnly Sunrise { get; set; }
}