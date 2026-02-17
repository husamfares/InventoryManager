using System.Runtime.CompilerServices;
using InventoryManager.Application.Interfaces;
using InventoryManager.Infrastructure.Data;

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


// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Enjection
builder.Services.AddScoped<IProductRepository,ProductRepository>();



var app = builder.Build();

//swagger
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Middleware pipeline
// app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");  
app.UseAuthorization();
app.MapControllers();

app.Run();
