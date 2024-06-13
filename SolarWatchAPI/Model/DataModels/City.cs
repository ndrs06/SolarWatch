using System.ComponentModel.DataAnnotations;

namespace SolarWatchAPI.Model.DataModels;

public class City
{
    [Key]
    public string Name { get; init; }
    public double Lat { get; init; }
    public double Lon { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
    
    public ICollection<SunriseSunset> SunriseSunsets { get; init; }
}