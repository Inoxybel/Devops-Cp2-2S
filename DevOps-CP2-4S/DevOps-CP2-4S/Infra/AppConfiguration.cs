using Domain.Interfaces.Infra;
using Infra;
using Infra.Options;
using Infra.Repositories;
using Microsoft.OpenApi.Models;

namespace DevOps_CP2_4S.Infra;

public static class AppConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "IAcademy User API", Version = "v1" });

            c.EnableAnnotations();
        });

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseInstanceOptions>(x =>
        {
            x.Name = configuration.GetValue<string>($"UserManager:Mongo:{DatabaseInstanceOptions.DatabaseNameConfigKey}");
            x.ConnectionString = configuration.GetValue<string>($"UserManager:Mongo:{DatabaseInstanceOptions.ConnectionStringConfigKey}");
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddSingleton<DbContext>();
        return services;
    }
}
