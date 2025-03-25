using JoyCase.Application.User.Query.LoginUserQuery;
using JoyCase.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public AuthController(IConfiguration configuration, IMediator mediator)
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserQuery request)
    {
        var response = await _mediator.Send(request);

        if (response == null || response.Data == null)
            return Unauthorized("Geçersiz kullanıcı adı veya şifre");

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, response.Data.UserId.ToString()), // kullanici id
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, response.Data.RoleId.ToString()) // kullanici rol id
        };

        // kullanicinin sahip olduğu tun yetkiler
        var permissions = response.Data.UserPermissions
            .Select(p => p.Name)
            .Union(response.Data.RolePermissions.Select(p => p.Name)) // direkt yetkileri cek
            .Distinct();

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }

        var key = new SymmetricSecurityKey(secretKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiresIn = jwtSettings["ExpiryInMinutes"]
        });
    }
}
