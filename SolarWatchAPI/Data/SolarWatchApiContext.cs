using dotenv.net;
using Microsoft.EntityFrameworkCore;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Data;

public class SolarWatchApiContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }
    private readonly IConfiguration _configuration;

    public SolarWatchApiContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MSSQL_CONNECTION"));
    }
}