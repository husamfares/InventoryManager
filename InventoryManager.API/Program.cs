using InventoryManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddControllers();

// CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger (optional)
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");  
app.UseAuthorization();
app.MapControllers();

app.Run();
