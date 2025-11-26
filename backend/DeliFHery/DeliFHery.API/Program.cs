using DeliFHery.API.Database; 
using DeliFHery.API.Interfaces;
using DeliFHery.API.Repo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection")!;
builder.Services.AddSingleton<IDbConnectionFactory>(c => new DbConnectionFactory(connectionString));
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();

var app = builder.Build();
app.MapControllers();
app.Run();
