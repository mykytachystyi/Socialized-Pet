using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Domain.Users;
using Domain.Enums;

namespace WebAPI.Middleware;

public interface IJwtTokenManager
{
    string Authenticate(User user);
}
public class JwtTokenManager : IJwtTokenManager
{
    private readonly IConfiguration _configuration;
    private readonly byte[] _keyBytes;
    
    public JwtTokenManager(IConfiguration configuration)
    {
        _configuration = configuration;
        var key = _configuration.GetValue<string>("JwtConfig:Key")
            ?? throw new InvalidOperationException("Jwt config key - відсутній.");
        _keyBytes = Encoding.UTF8.GetBytes(key);
    }
    public string Authenticate(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
             [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, IdentityRoleConverter.CovertToRoleName(user.Role))
             ]),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                                  new SymmetricSecurityKey(_keyBytes),
                                  SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}