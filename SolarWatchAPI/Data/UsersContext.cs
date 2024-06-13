using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SolarWatchAPI.Data;

public class UsersContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    private readonly IConfiguration _configuration;
    
    public UsersContext(DbContextOptions<UsersContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("MSSQL_CONNECTION"));
    }
}