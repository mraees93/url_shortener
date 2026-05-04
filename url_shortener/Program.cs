using url_shortener.Database;
using Microsoft.EntityFrameworkCore;
using url_shortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=urlshortener.db"));

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<UrlShortenerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

//app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();
