using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PTO_Server.Extensions
{
    public static class ServiceExtensions
    {   
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder => 
                                                builder
                                                .AllowAnyOrigin()
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()                                            
                                                )
            );
        }

        public static void ConfigureJWTBearer(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://localhost:7152",// getPTO_AppConfigurations(),
                        ValidAudience = getPTO_AppConfigurations(), //"http://localhost:4200",// getPTO_AppConfigurations(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                    };
                });
        }

        private static string getPTO_AppConfigurations()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var applicationUrl = MyConfig.GetSection("PTO_App")["applicationUrl"];
            return applicationUrl.ToString();
        }
    }
}
