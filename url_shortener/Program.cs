using url_shortener.Database;
using Microsoft.EntityFrameworkCore;
using url_shortener.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | 
                               ForwardedHeaders.XForwardedProto | 
                               ForwardedHeaders.XForwardedHost;
    
    options.KnownIPNetworks.Clear(); 
    options.KnownProxies.Clear();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (connectionString != null && connectionString.Contains("Host="))
    {
        // Use PostgreSQL for Neon (Production)
        options.UseNpgsql(connectionString);
    }
    else
    {
        // Default to SQLite (Local Development)
        options.UseSqlite(connectionString);
    }
});

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
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

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

app.UseForwardedHeaders();
app.UseCors();
app.UseRateLimiter();
app.MapControllers();

// AUTOMATIC MIGRATION LOGIC
using (var scope = app.Services.CreateScope())
{
    try 
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // This handles both SQLite (local) and Postgres (Neon) automatically
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log the error but let the app start (avoids Render crash loops)
        Console.WriteLine($"Migration Error: {ex.Message}");
    }
}

app.Run();
