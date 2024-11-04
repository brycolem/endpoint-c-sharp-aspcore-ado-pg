using CSharpAspCoreAdoPg.Configurations;
using CSharpAspCoreAdoPg.Services;
using CSharpAspCoreAdoPg.Wrappers;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Limits.MaxConcurrentConnections = 1000;
            serverOptions.Limits.MaxConcurrentUpgradedConnections = 1000;
            serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(3);
        });

        ThreadPool.SetMinThreads(50, 50);
        ThreadPool.SetMaxThreads(100, 100);

        builder.Configuration.AddEnvironmentVariables();

        var databaseConfiguration = new DatabaseConfiguration(builder.Configuration);
        builder.Services.AddSingleton(databaseConfiguration);
        builder.Services.AddSingleton<IDbConnectionFactory>(serviceProvider =>
        {
            var dbConfiguration = serviceProvider.GetRequiredService<DatabaseConfiguration>();
            return new DbConnectionFactory(dbConfiguration.ConnectionString);
        });
        builder.Services.AddScoped<IApplicationService, ApplicationService>();
        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.Run("http://0.0.0.0:8001");
    }
}
