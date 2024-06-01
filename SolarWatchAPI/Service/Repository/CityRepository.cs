using SolarWatchAPI.Data;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Service.Repository;

public class CityRepository : ICityRepository
{
    private readonly SolarWatchApiContext _dbContext;

    public CityRepository(SolarWatchApiContext context)
    {
        _dbContext = context;
    }
    
    public IEnumerable<City> GetAll()
    {
        return _dbContext.Cities.ToList();
    }

    public City? GetByName(string name)
    {
        return _dbContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void Add(City city)
    {
        _dbContext.Add(city);
        _dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        _dbContext.Remove(city);
        _dbContext.SaveChanges();
    }

    public void Update(City city)
    {
        _dbContext.Update(city);
        _dbContext.SaveChanges();
    }
}