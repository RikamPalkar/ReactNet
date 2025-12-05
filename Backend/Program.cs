using Backend.Data;                      // Imports the AppDbContext class from your Data folder
using Microsoft.EntityFrameworkCore;      // Enables Entity Framework Core functionality

var builder = WebApplication.CreateBuilder(args);  
// Creates a new WebApplication builder (sets up configuration, logging, DI container, etc.)

// Add services
builder.Services.AddControllers();  
// Adds controller support (API endpoints)

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Data Source=trades.db")
);
// Registers AppDbContext with dependency injection and configures SQLite as the database provider.
// Retrieves the connection string from appsettings.json; fallback is a local trades.db file.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
// Configures CORS so your frontend (e.g., React/Angular) can call the API without restriction.

builder.Services.AddEndpointsApiExplorer();  
// Enables discovery of minimal API endpoints for Swagger documentation

builder.Services.AddSwaggerGen();  
// Adds Swagger/OpenAPI generation for API documentation UI

var app = builder.Build();  
// Builds the WebApplication pipeline (middleware + DI + routing)

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); 
    // Applies any pending EF Core migrations at startup (creates the database if not existing)
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      
    // Enables Swagger middleware (JSON spec endpoint) in development

    app.UseSwaggerUI();   
    // Enables the interactive Swagger UI to test API endpoints
}

app.UseCors("AllowFrontend");  
// Applies the CORS policy defined earlier to incoming requests

app.UseHttpsRedirection();  
// Redirects HTTP requests to HTTPS

app.UseAuthorization();  
// Enables authorization middleware (checks [Authorize] attributes, roles, etc.)

app.MapControllers();  
// Maps routes to controller endpoints

app.Run();  
// Starts the application and begins listening for HTTP requests