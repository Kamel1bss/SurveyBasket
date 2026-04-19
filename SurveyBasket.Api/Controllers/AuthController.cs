namespace SurveyBasket.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
        
        return authResult is null ? BadRequest("Invalid login attempt.") : Ok(authResult);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.RegisterAsync(request, cancellationToken);
        return authResult is null ? BadRequest("Registration failed.") : Ok(authResult);
    }
}
