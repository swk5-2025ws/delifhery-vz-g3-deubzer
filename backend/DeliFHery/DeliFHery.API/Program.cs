using DeliFHery.API.Database; 
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using DeliFHery.API.Repo;
using DeliFHery.API.Services;
using DeliFHery.API.Services.PaymentNamespace;
using DeliFHery.API.Services.Pricing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
var authority = keycloak["Authority"]!;
var audience = keycloak["Audience"]!;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authority;
        options.Audience = audience;
        options.RequireHttpsMetadata = bool.Parse(keycloak["RequireHttpsMetadata"] ?? "false");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudiences = new[] { audience },
            ValidIssuers = new[]
          {
              authority, // http://keycloak:8080/realms/delifhery
              "http://localhost:8080/realms/delifhery",
              "http://127.0.0.1:8080/realms/delifhery"
          }
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
builder.Services.AddScoped<IPaymentRepo, PaymentRepo>(); 
builder.Services.AddScoped<ITrackingEventRepo, TrackingEventRepo>();
builder.Services.AddScoped<ICarrierAuthRepo, CarrierAuthRepo>();
builder.Services.AddScoped<ITrackingEventRepo, TrackingEventRepo>();
builder.Services.AddScoped<ICarrierTrackingService, CarrierTrackingService>();
builder.Services.AddScoped<INotificationSubscriptionRepo, NotificationSubscriptionRepo>();
builder.Services.AddScoped<IEmailSender, EmailSenderService>();
builder.Services.AddScoped<ICarrierRepo, CarrierRepo>();


builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));

builder.Services.Configure<PaymentOptions>(
    builder.Configuration.GetSection("Payment"));

builder.Services.AddHttpClient<IPaymentService, PaymentService>();

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
