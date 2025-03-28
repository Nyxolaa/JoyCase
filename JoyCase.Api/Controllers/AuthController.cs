using JoyCase.Api.Log;
using JoyCase.Application.User.Query.LoginUserQuery;
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
    private readonly LogService _logService;

    public AuthController(IConfiguration configuration, IMediator mediator)
    {
        _configuration = configuration;
        _mediator = mediator;
        _logService = new LogService();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserQuery request)
    {
        var response = await _mediator.Send(request);
        if (response.Data != null)
        {
            if (response == null || response.Data == null)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre");

            var jwtSettings = _configuration.GetSection("JwtSettings");

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

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"], 
                Audience = jwtSettings["Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            //var key = new SymmetricSecurityKey(secretKey);
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var token = new JwtSecurityToken(
            //    issuer: jwtSettings["Issuer"],
            //    audience: jwtSettings["Audience"],
            //    claims: claims,
            //    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
            //    signingCredentials: creds
            //);

            //if (response.Data == null)
            //{
            //    return NotFound(response.Errors); // Bu durumda 'NotFound' döneer, 'OkObjectResult' değil.
            //}
            return Ok(new
            {
                token = jwtToken,
                expiresIn = int.TryParse(jwtSettings["ExpiryInMinutes"], out var expiry) ? expiry : 60
            });
        }
        else
        {
            return NotFound(response.Errors ?? new string[] { "Bilinmeyen hata oluştu" });
        }
    }

    [HttpGet("log-test")]
    public IActionResult LogTest()
    {
        _logService.LogInfo("Bu bir bilgi mesajıdır.");
        _logService.LogWarning("Bu bir uyarı mesajıdır.");
        _logService.LogError("Bu bir hata mesajıdır.", new Exception("Örnek hata"));

        return Ok("Log test işlemi tamamlandı.");
    }
}
