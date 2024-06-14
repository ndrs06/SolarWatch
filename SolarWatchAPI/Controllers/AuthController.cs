using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Contracts;
using SolarWatchAPI.Model.Authentication;
using SolarWatchAPI.Service.Authentication;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authenticationService, IConfiguration configuration)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
    }
    
    [HttpPost("Registration")]
    public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var result = await _authenticationService.RegisterAsync(request.Email, request.UserName, request.Password, jwtSettings.GetSection("Roles").Get<string[]>()[0]);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.LoginAsync(request.Email, request.Password);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return Ok(new AuthResponse(result.Email, result.UserName, result.Token));
    }
    
    private void AddErrors(AuthResult result)
    {
        foreach (var err in result.ErrorMessages)
        {
            ModelState.AddModelError(err.Key, err.Value);
        }
    }
}