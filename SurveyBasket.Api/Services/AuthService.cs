using Microsoft.AspNetCore.Identity;
using SurveyBasket.Api.Authentication;

namespace SurveyBasket.Api.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;   
    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
    {
        // get user by email
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
            return null;

        var (token, expiration) = _jwtProvider.GenerateToken(user);
        
        return new AuthResponse
        (
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token,
            expiration
        );
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
    
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new RegisterResponse
            (
                request.Email,
                request.FirstName,
                request.LastName,
                $"User registration failed: {errors}"
            );
        }

        return new RegisterResponse
        (
            request.Email,
            request.FirstName,
            request.LastName,
            "User registered successfully"
        );

    }
}
