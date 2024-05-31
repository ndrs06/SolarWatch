using System.ComponentModel.DataAnnotations;

namespace SolarWatchAPI.Model.DataModels;

public class City
{
    [Key]
    public int Id { get; init; }
    public string Name { get; init; }
    public double Lat { get; init; }
    public double Lon { get; init; }
    public string State { get; init; }
    public string Coutry { get; init; }
    
    public ICollection<SunriseSunset> SunriseSunsets { get; init; }
}