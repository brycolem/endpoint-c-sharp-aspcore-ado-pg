using CSharpAspCoreAdoPg.Configurations;
using CSharpAspCoreAdoPg.Services;

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

var databaseConfig = new DatabaseConfiguration(builder.Configuration);
builder.Services.AddSingleton(databaseConfig);
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
