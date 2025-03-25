using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

public static class JwtService
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        // launchSettings den Secret Key al
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("JWT Secret bulunamadı! Environment variable olarak ayarlayın.");
        }

        var key = Encoding.UTF8.GetBytes(secretKey);

        // JWT Authentication
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
    }
}
