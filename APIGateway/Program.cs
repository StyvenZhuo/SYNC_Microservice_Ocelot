using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfigurasi
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// 2. Service registration (semua AddXxx harus di sini)
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("https://localhost:5173")
            .AllowCredentials());
});

// 3. Build app
var app = builder.Build();

// 4. Middleware (UseXxx)
await app.UseOcelot();
app.UseCors("CorsPolicy"); // Jangan lupa tambahkan ini untuk mengaktifkan CORS

app.Run();