using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Data;

public class SolarWatchApiContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }
    private readonly IConfiguration _configuration;

    public SolarWatchApiContext(DbContextOptions<SolarWatchApiContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MSSQL_CONNECTION"));
    }
}