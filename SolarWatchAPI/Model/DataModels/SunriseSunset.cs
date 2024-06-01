using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatchAPI.Model.DataModels;

public class SunriseSunset
{
    [Key]
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public TimeOnly Sunrise { get; init; }
    public TimeOnly Sunset { get; init; }
    
    [ForeignKey("City")]
    public int CityId { get; init; }
    public City City { get; init; }
}