using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatchAPI.Model.DataModels;

public class SunriseSunset
{
    [Key]
    public int Id { get; init; }
    public DateTime Sunrise { get; init; }
    public DateTime Sunset { get; init; }
    
    [ForeignKey("City")]
    public int CityId { get; init; }
    public City City { get; init; }
}