using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Properties.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // This registers all the services needed for MVC controllers to work

// Register ShirtContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger for API documentation
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebAPIDemo API", Version = "v1" });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Middleware pipeline - correct order
app.UseRouting();
app.UseAuthorization();

// Minimal API routes (commented out since you're using controllers)
//app.MapGet("/shirts", () =>
//{
//    return "Reading all the shirts";    
//});
//// get a particular shirt  
//app.MapGet("/shirts/{id}", (int id) =>
//{
//    return $"Reading shirt with id {id}";
//});
//// creating a shirt 
//app.MapPost("/shirts", (string shirt) =>
//{
//    return $"Creating a shirt with name {shirt}";
//});
//// updating a shirt
//app.MapPut("/shirts/{id}", (int id, string shirt) =>
//{
//    return $"Updating shirt with id {id} to {shirt}";
//});
//// deleting a shirt 
//app.MapDelete("/shirts/{id}", (int id) =>
//{
//    return $"Deleting shirt with id {id}";
//});

// Map controllers - this maps all your controller routes
app.MapControllers();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}