using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatchAPI.Data;

namespace SolarWatchAPIIntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    /*private readonly string _dbSolarWatch = Guid.NewGuid().ToString();

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
        });
    }*/
}