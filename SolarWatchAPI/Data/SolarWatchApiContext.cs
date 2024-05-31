using dotenv.net;
using Microsoft.EntityFrameworkCore;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPI.Data;

public class SolarWatchApiContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }
    private readonly string _connectionString;

    public SolarWatchApiContext()
    {
        DotEnv.Load();
        var enVars = DotEnv.Read();
        _connectionString = enVars["MSSQL_CONNECTION"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasIndex(u => u.Name)
            .IsUnique();
        
        modelBuilder.Entity<SunriseSunset>()
            .HasOne(s => s.City)
            .WithMany(c => c.SunriseSunsets)  // Defines the one-to-many relationship
            .HasForeignKey(s => s.CityId);
    }
}