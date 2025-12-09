using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;



// lastige code. overal comments bijgezet om het nog een beetje duidelijk te maken
public class JwtTokenService
{
    private readonly IConfiguration _config;

    
    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user, IList<string> roles)
    {
        // claims maken en toevoegen 
        var claims = new List<Claim>();
        // id
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        // email 
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

        // rollen toevoegen
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // sleutel ophalen uit appsettings.json
        var secretKey = _config["Jwt:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        // signing credentials maken met algoritme HMAC-SHA256. dit geeft aan hoe de token gesigneerd wordt
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // token maken voor user
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"], // naam van de token uitgever
            audience: _config["Jwt:Audience"], // naam van de token ontvanger
            claims: claims, // toegevoegde claims
            expires: DateTime.Now.AddHours(2), // vervaltijd van de token
            signingCredentials: signingCredentials // de signing credentials
        );

        // token naar string
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
