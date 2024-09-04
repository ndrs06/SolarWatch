using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatchAPI.Data;
using SolarWatchAPI.Model.DataModels;

namespace SolarWatchAPIIntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbSolarWatch = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => 
                    d.ServiceType == typeof(DbContextOptions<SolarWatchApiContext>));

            if (solarWatchDbContextDescriptor != null)
            {
                services.Remove(solarWatchDbContextDescriptor);
            }
            
            services.AddDbContext<SolarWatchApiContext>(options =>
            {
                options.UseInMemoryDatabase(_dbSolarWatch);
            });
            
            using var scope = services.BuildServiceProvider().CreateScope();
            
            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchApiContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();
            
            SeedData(solarContext);
        });
    }
    
    private void SeedData(SolarWatchApiContext context)
    {
        var city1 = new City
        {
            Name = "Miskolc",
            State = "Borsod-Abaúj-Zemplén",
            Country = "Hungary",
            Lat = 48.1030,
            Lon = 20.7789
        };

        var city2 = new City
        {
            Name = "Zamárdi",
            State = "Somogy",
            Country = "Hungary",
            Lat = 46.8820,
            Lon = 17.9400
        };
        
        var sunriseSunset1 = new SunriseSunset
        {
            CityName = "Miskolc",
            Date = DateTime.UtcNow.Date,
            Sunrise = new TimeOnly(6, 0),
            Sunset = new TimeOnly(18, 0)
        };

        var sunriseSunset2 = new SunriseSunset
        {
            CityName = "Zamárdi",
            Date = DateTime.UtcNow.Date,
            Sunrise = new TimeOnly(6, 30),
            Sunset = new TimeOnly(18, 30)
        };
        
        context.Cities.AddRange(city1, city2);
        context.SunriseSunsets.AddRange(sunriseSunset1, sunriseSunset2);
        
        context.SaveChanges();
    }
}