using Identity.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOption>(configuration.GetSection("JwtBearer"));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var signinKey = Encoding.UTF32.GetBytes("qwertyuiopasdfghjklzxcvbnm124567890");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "Identity.Api",
                    ValidAudience = "Products",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signinKey),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}