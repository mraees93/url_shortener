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

// 1. DUAL-SOURCE CONNECTION CHECK
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

// 2. HARDENED DB CONFIGURATION
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Host="))
    {
        Console.WriteLine(">>>> SYSTEM CHECK: CONNECTION TO NEON POSTGRES DETECTED <<<<");
        // Force version 16 to ensure compatibility with Neon's engine
        options.UseNpgsql(connectionString, o => o.SetPostgresVersion(16, 0));
    }
    else
    {
        Console.WriteLine(">>>> SYSTEM CHECK: FALLING BACK TO SQLITE <<<<");
        options.UseSqlite(connectionString ?? "Data Source=vertex.db");
    }
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
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
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.UseForwardedHeaders();
app.UseCors();
app.UseRateLimiter();
app.MapControllers();

// 3. SECURE AUTOMATIC MIGRATION LOGIC
using (var scope = app.Services.CreateScope())
{
    try 
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Console.WriteLine(">>>> SYSTEM CHECK: STARTING DATABASE MIGRATIONS <<<<");
        db.Database.Migrate();
        Console.WriteLine(">>>> SYSTEM CHECK: MIGRATIONS COMPLETE <<<<");
    }
    catch (Exception ex)
    {
        // This captures the 28P01 error if it still occurs
        Console.WriteLine($">>>> SYSTEM CHECK: MIGRATION ERROR: {ex.Message} <<<<");
    }
}

app.Run();
