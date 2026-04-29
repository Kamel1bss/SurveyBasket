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

        services.AddAuthConfig(_config);

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
    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration _config)
    {

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddSingleton<IJwtProvider, JwtProvider>();
        // services.Configure<JwtOptions>(_config.GetSection(JwtOptions.SectionName));
        services.AddOptions<JwtOptions>()
            .Bind(_config.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtOptions = _config.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

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
                    ValidIssuer = jwtOptions?.Issuer,
                    ValidAudience = jwtOptions?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.Key!))
                };
            });
        return services;
    }

}
