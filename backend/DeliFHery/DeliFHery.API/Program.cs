using DeliFHery.API.Database; 
using DeliFHery.API.Interfaces;
using DeliFHery.API.Repo;

using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddCors(option =>
{
    option.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var keycloak = builder.Configuration.GetSection("KeyCloak");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloak["Authority"];
        options.Audience = keycloak["Audience"];
        options.RequireHttpsMetadata = bool.Parse(keycloak["RequireHttpsMetadata"] ?? "false");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });


builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection")!;
builder.Services.AddSingleton<IDbConnectionFactory>(c => new DbConnectionFactory(connectionString));
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IContactMethodRepo, ContactMethodRepo>();

var app = builder.Build();
app.UseCors("AngularClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
