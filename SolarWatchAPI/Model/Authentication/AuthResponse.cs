namespace SolarWatchAPI.Model.Authentication;

public record AuthResponse(string Email, string UserName, string Token);