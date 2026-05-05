using url_shortener.Database;
using Microsoft.EntityFrameworkCore;
using url_shortener.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

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

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1); // The time window
        opt.PermitLimit = 10;                // Max requests per window
        opt.QueueLimit = 0;                   // Don't queue extra requests, just reject them
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Custom response when limited
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new { error = "Too many requests. Try again in a minute." }, token);
    };
});

var app = builder.Build();

app.UseMiddleware<url_shortener.Middleware.ExceptionMiddleware>();

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

app.UseRateLimiter();

app.MapControllers();

//ensures Neon tables are created automatically on deploy
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<url_shortener.Database.AppDbContext>();
    db.Database.Migrate();
}

app.Run();
