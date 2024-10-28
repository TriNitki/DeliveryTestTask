using Delivery.DataAccess;
using Delivery.Service.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Program = Delivery.Service.Program;

namespace Delivery.FunctionalTests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = GetContainer();

    private const string TestDbProvider = "TestNpgsql";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<Context>));

            services.AddDataContext<Context>(_dbContainer.GetConnectionString());
        });
    }

    private static PostgreSqlContainer GetContainer()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        if (config == null)
            throw new ArgumentNullException(nameof(config));

        var csb = new SqlConnectionStringBuilder(config.GetConnectionString(TestDbProvider)
                                                 ?? throw new ArgumentNullException(null, "Test data base connection string is not specified"));

        return new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase(csb.InitialCatalog)
            .WithUsername(csb.UserID)
            .WithPassword(csb.Password)
            .Build();
    }

    public DbContextOptions<Context> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<Context>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .UseUpperCaseNamingConvention()
            .Options;
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}