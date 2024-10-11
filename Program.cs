var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Comment out HTTPS redirection since we're using HTTP
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure the application runs on HTTP port 8001
app.Run("http://localhost:8001");
