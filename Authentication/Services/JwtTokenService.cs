using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;

    // private readonly string _keyId;


    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["JwtSettings:Secret"];

        // _keyId = Convert.ToBase64String(SHA256.HashData(rsa.ExportParameters(false).Modulus));
    }

    public string GenerateToken(long userId, string role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        DateTime expireTime = DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"]));

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: new DateTimeOffset(expireTime).DateTime,
            signingCredentials: signingCredentials
        );

        // token.Header["kid"] = _keyId;

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}