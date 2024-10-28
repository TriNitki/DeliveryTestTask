using System.Reflection;
using System.Text.Json.Serialization;
using Delivery.Audit.Contracts;
using Delivery.Audit.Logger.Extensions;
using Delivery.Audit.UseCases;
using Delivery.Audit.UseCases.Abstractions;
using Delivery.DataAccess;
using Delivery.DataAccess.Repositories;
using Delivery.Service.Infrastructure;
using Delivery.UseCases;
using Delivery.UseCases.District;
using Delivery.UseCases.Orders;
using Delivery.UseCases.Orders.Queries;
using NLog;
using NLog.Web;

namespace Delivery.Service;

public class Program
{
    public static async Task Main(string[] args)
    {
        var logger = LogManager.Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();

        try
        {
            logger.Debug("init main");
            var builder = ConfigureApp(args);
            logger.Debug("Application configured");
            await RunApp(builder);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred when starting the host");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static WebApplicationBuilder ConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        var services = builder.Services;

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        var basePath = AppContext.BaseDirectory;
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        services.AddSwaggerGen(opts =>
        {
            opts.IncludeXmlComments(Path.Combine(basePath, xmlFile), includeControllerXmlComments: true);

            opts.EnableAnnotations();
            opts.UseAllOfToExtendReferenceSchemas();
            opts.UseAllOfForInheritance();
            opts.UseOneOfForPolymorphism();
            opts.UseInlineDefinitionsForEnums();

            opts.SelectDiscriminatorNameUsing(_ => "$type");
            opts.SelectDiscriminatorValueUsing(subType => subType.BaseType!
                .GetCustomAttributes<JsonDerivedTypeAttribute>()
                .FirstOrDefault(x => x.DerivedType == subType)?
                .TypeDiscriminator!.ToString());
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddHealthChecks();

        ConfigureDI(services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([
            typeof(GetDeliveryOrderQuery).Assembly,
            typeof(CreateAuditLogCommand).Assembly,
            typeof(CreateAuditLogCommandHandler).Assembly
        ]));
        services.AddValidationPipelines(typeof(GetDeliveryOrderQueryValidator).Assembly);
        services.AddDataContext<Context>(configuration);

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IDeliveryOrderRepository, DeliveryOrderRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        services.AddAuditLogging();
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<Context>();
        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        app.UseRouting();
        app.UseCors();

        app.MapHealthChecks("/health").AllowAnonymous();

        app.UseAuditLogging();

        app.MapControllers();

        await app.RunAsync();
    }
}