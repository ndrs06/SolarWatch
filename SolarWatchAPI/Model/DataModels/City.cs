using System.ComponentModel.DataAnnotations;

namespace SolarWatchAPI.Model.DataModels;

public class City
{
    [Key]
    public string Name { get; init; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    
    public ICollection<SunriseSunset> SunriseSunsets { get; init; }
}