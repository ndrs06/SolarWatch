using SolarWatchAPI.Data;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service.Repository;

public class SunsetSunriseRepository : ISunriseSunsetRepository
{
    private readonly SolarWatchApiContext _dbContext;

    public SunsetSunriseRepository(SolarWatchApiContext context)
    {
        _dbContext = context;
    }
    
    public IEnumerable<SunriseSunset> GetAll()
    {
        return _dbContext.SunriseSunsets.ToList();
    }

    public SunriseSunset? GetByCityAndDate(string cityName, DateTime date)
    {
        return _dbContext.SunriseSunsets.FirstOrDefault(s => s.CityName == cityName && s.Date == date);
    }

    public void Add(SunriseSunset sunriseSunset)
    {
        // TODO
        // _dbContext.SunriseSunsets.Add(sunriseSunset); ???????
        
        _dbContext.Add(sunriseSunset);
        _dbContext.SaveChanges();
    }

    public void Delete(SunriseSunset sunriseSunset)
    {
        _dbContext.Remove(sunriseSunset);
        _dbContext.SaveChanges();
    }

    public void Update(SunriseSunset sunriseSunset)
    {
        _dbContext.Update(sunriseSunset);
        _dbContext.SaveChanges();
    }
}