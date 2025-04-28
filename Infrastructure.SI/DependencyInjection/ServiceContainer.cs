

using Application.SI.Extension.Identity;
using Application.SI.Interface.Identity;
using Infrastructure.SI.DataAccess;
using Infrastructure.SI.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SI.DependencyInjection;

public static class ServiceContainer 
{

    public static IServiceCollection AddInfrastructureService(
        this IServiceCollection services, IConfiguration config)
    {
        //Añadimos El DBContext

        services.AddDbContext<AppDbContext>(o => o.UseSqlServer
        (config.GetConnectionString("Default")), ServiceLifetime.Scoped);
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddAuthorizationBuilder()
            .AddPolicy("AdministrationPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("Admin", "Manager");
                })
            .AddPolicy("UserPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("User");
                });
        services.AddScoped<IAccount, Account>();
        return services;
    }
}
