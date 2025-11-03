using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// .NET 9 built-in OpenAPI (produces /openapi/v1.json)
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Serve OpenAPI JSON (v1)
    app.MapOpenApi();

    // Scalar UI (nice modern viewer for the OpenAPI doc)
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("DeliFHery API");
        options.WithTheme(ScalarTheme.Mars); // 
    }); // UI at /scalar
}

// usual pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// optional: redirect root to the UI
app.MapGet("/", () => Results.Redirect("/scalar"));

app.Run();
