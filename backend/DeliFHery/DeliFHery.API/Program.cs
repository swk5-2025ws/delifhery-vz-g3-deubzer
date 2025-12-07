using DeliFHery.API.Database; 
using DeliFHery.API.Interfaces;
using DeliFHery.API.Repo;
using DeliFHery.API.Services;
using DeliFHery.API.Services.Payment;
using DeliFHery.API.Services.Pricing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
builder.Services.AddScoped<IShippingPriceCalculator, ShippingPriceCalculator>();
builder.Services.AddScoped<IShippingPriceRule, BasePriceRule>();
builder.Services.AddScoped<IShippingPriceRule, StateSurChargeRule>();
builder.Services.AddScoped<IShippingPriceRule, MonthDiscountRule>();
builder.Services.AddSingleton<IRouteService, RouteService>();
builder.Services.AddScoped<IShipmentRepo, ShipmentRepo>();
builder.Services.AddScoped<IShipmentPriceRepo, ShipmentPriceRepo>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ILabelGenerator, LabelGenerator>();

builder.Services.AddOpenApiDocument(
    settings => settings.Title = "DeliFHery API");

var app = builder.Build();
app.UseCors("AngularClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseOpenApi();
app.UseSwaggerUi(settings => settings.Path = "/swagger");

app.Run();
