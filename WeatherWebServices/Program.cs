 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using WeatherWebServices.Data;
using WeatherWebServices.Models;

var builder = WebApplication.CreateBuilder(args);



 

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


builder.Services.AddScoped<TokenService>();

//  . Get Connection String
var connectionString = builder.Configuration.GetConnectionString("WeatherDBConnection");


// Bind the "WeatherApi" section from appsettings.json to the WeatherSettings class
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherApi"));



// Register the repository with the connection string
builder.Services.AddScoped<WeatherForecastRepository>(provider =>
    new WeatherForecastRepository(connectionString));



// Register the repository with the connection string
builder.Services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>(provider =>
    new WeatherForecastRepository(connectionString));

// Register the repository with the connection string
builder.Services.AddScoped<WeatherReadingsRepository>(provider =>
    new WeatherReadingsRepository(connectionString));


// Register Service
builder.Services.AddHttpClient<WeatherReadingsService>();

// Register Service
builder.Services.AddHttpClient<WeatherForecastService>();



// Register the repository with the connection string
builder.Services.AddHostedService<BackgroundDataFetchServices>();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // This will hide null values
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
}); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    // 1. Define the security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token: Bearer {your_token}"
    });

    // 2. Apply the security scheme globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
  
});


var app = builder.Build();


 


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Forecast API V1");
    
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
