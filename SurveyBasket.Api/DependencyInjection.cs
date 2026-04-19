using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Persistence;
using System.Reflection;
using System.Text;

namespace SurveyBasket.Api;

public static class DependencyInjection
{
    
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration _config)
    {
        services.AddControllers();

        services.AddApplicationDbContext(_config);

        services.AddAuthConfig();

        services
            .AddSwggerServices()
            .AddMapsterConfig()
            .AddFluentValidationConfig();

        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

    private static IServiceCollection AddSwggerServices(this IServiceCollection services)
    {
        services
            .AddOpenApi()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mappingConfing = TypeAdapterConfig.GlobalSettings;
        mappingConfing.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfing));

        return services;
    }
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
    private static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration _config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(_config.GetConnectionString("DefualtConnection")));
        

        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).
            AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "SurveyBasketApp",
                    ValidAudience = "SurveyBasketApp Users",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("095ea79a4c7e8c998b2c658b2fbeb99f39b2ad929f11d409a4f0"))
                };
            });
        return services;
    }

}
