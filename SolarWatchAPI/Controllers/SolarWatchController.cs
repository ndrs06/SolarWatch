using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;

    public SolarWatchController(ILogger<SolarWatchController> logger)
    {
        _logger = logger;
    }



    [HttpGet(Name = "SolarWatch")]
    public ActionResult<SolarWatch> Get()
    {
        try
        {
            return new SolarWatch 
            { 
                Date = DateTime.Now, 
                City = "Miskolc",
                Sunset = new TimeOnly(6, 30), 
                Sunrise = new TimeOnly(20, 30) 
            };
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting data");
            return NotFound("Error getting data");
        }
    }
}