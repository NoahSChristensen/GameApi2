// var builder = WebApplication.CreateBuilder(args);
// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// var app = builder.Build();
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
// app.UseHttpsRedirection();
// var summaries = new[]
// {
//  "Freezing" , "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot",
// "Sweltering" , "Scorching"
// };
// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//     new WeatherForecast
//     (
//     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//     Random.Shared.Next(-20, 55),
//     summaries[Random.Shared.Next(summaries.Length)]
//     ))
//     .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");
// // .WithOpenApi();
// app.Run();
// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }




using DotNetEnv;
using GameApi2.Controllers;
using GameApi2.Data;
using GameApi2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Load .env automatisk
Env.Load();
// Instantier url variablen
var url = "http://localhost:5212";
if (!string.IsNullOrEmpty(url))
{
    // Dette fortæller Kestrel, hvilken port den skal lytte på
    builder.WebHost.UseUrls(url);
}
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at
//https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Description = "JWT. Indtast kun din token (Swagger tilføjer ´Bearer´ automatisk)",
        Name = "Authorization",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });

});

builder.Services.AddSingleton<GameApi2.Services.DataService>();
builder.Services.AddScoped<GameApi2.Repositories.IUserRepository, GameApi2.Repositories.UserRepository>();
builder.Services.AddScoped<GameApi2.Services.UserService>();
builder.Services.AddScoped<GameApi2.Services.AuthService>();

// CORS – nødvendigt så mobil-appen (Expo Go) kan kalde API'et fra telefonen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApp", policy =>
    {
        policy.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader();
    });
});

// JWT                              jwt: Key
var jwtKey = builder.Configuration["jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key mangler i appsettings.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            // Required so RequireRole("Admin") recognizes the role from JWT
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});


builder.Services.AddScoped<AuthService>();

// // Register repositories and services
// builder.Services.AddScoped<GameApi2.Repositories.IUserRepository,
// GameApi2.Repositories.UserRepository>();
// builder.Services.AddScoped<GameApi2.Services.UserService>();
// builder.Services.AddSingleton<DataService>();

builder.Services.AddDbContext<DbContextGameApi>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data -Source-gameapi.db")
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<DbContextGameApi>().Database.EnsureCreated();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowApp"); // eller UseCors("AllowApp") hvis du navngav policy
app.UseAuthentication();
app.UseAuthorization();
// Map user endpoints
await app.MapUserEndpoints();
await app.MapAuthEndpoints();
app.Run();