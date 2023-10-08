using DevOps_CP2_4S.Infra;
using Domain.Interfaces.Services;
using Domain.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json.Serialization;

namespace DevOps_CP2_4S
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            ConfigureApp(app, builder.Configuration);

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            MongoConfiguration.RegisterConfigurations();

            services.AddScoped<IUserService, UserService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<UserRequestValidator>();

            services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = false;
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            services
                .AddSwagger()
                .AddOptions(configuration)
                .AddRepositories()
                .AddHealthChecks()
                .AddMongoDb(
                    mongodbConnectionString: configuration["UserManager:MongoDB:ConnectionString"] ?? "ConnectionString not founded",
                    name: "health-check-mongodb"
                );

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }

        public static void ConfigureApp(WebApplication app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");

                c.RoutePrefix = "swagger";

                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);

                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            app.UseCors();

            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();
        }
    }
}