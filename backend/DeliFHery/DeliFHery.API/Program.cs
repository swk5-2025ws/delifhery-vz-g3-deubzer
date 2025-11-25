using DeliFHery.API.Database;   // SqlConnectionFactory, DatabaseService
using DeliFHery.API.Interfaces; // ICustomerRepo
using DeliFHery.API.Repo;       // CustomerRepo

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var cs = builder.Configuration.GetConnectionString("DatabaseConnection")!;
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(cs));
builder.Services.AddSingleton<DatabaseService>(); // if you renamed Database → DatabaseService
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();

var app = builder.Build();
app.MapControllers();
app.Run();
