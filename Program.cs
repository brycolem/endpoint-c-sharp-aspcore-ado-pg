var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

string database = Environment.GetEnvironmentVariable("DATABASE") ?? "DefaultDatabase";
string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "DefaultUser";
string dbPassword = Environment.GetEnvironmentVariable("DB_PWD") ?? "DefaultPassword";
string dbHost = "postgres_bench";

string connectionString = $"Server={dbHost};Database={database};User Id={dbUser};Password={dbPassword};";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthorization();
app.MapControllers();

app.Run("http://0.0.0.0:8001");
