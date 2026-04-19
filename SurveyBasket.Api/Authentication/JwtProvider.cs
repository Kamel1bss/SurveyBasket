using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider : IJwtProvider
{
    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.GivenName, user.FirstName!),
            new Claim(ClaimTypes.Surname, user.LastName!),
        };

        var symmetricSecurityKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes("095ea79a4c7e8c998b2c658b2fbeb99f39b2ad929f11d409a4f0"));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var expiresIn = 30;
        var expirationDate = DateTime.UtcNow.AddMinutes(expiresIn);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: "SurveyBasketApp",
            audience: "SurveyBasketApp Users",
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return (token, expiresIn * 60);
    }
}
