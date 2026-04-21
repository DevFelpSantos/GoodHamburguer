using GoodHamburger.API.Configurations;
using GoodHamburger.API.Middlewares;
using GoodHamburger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Debug da connection string
Console.WriteLine("CONNECTION STRING:");
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

// Services
builder.Services.AddAppSwagger();
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middlewares
app.UseMiddleware<ExceptionMiddleware>();
app.UseAppSwagger();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// =============================
// MIGRATIONS COM RETRY
// =============================
ApplyMigrations(app);

app.Run();

// =============================
// MÉTODO DE MIGRATION
// =============================
static void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<GoodHamburgerContext>();

    var retries = 5;
    var delay = TimeSpan.FromSeconds(5);

    while (retries > 0)
    {
        try
        {
            Console.WriteLine("Aplicando migrations...");
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations aplicadas com sucesso.");
            break;
        }
        catch (Exception ex)
        {
            retries--;

            Console.WriteLine($"Erro ao conectar no banco: {ex.Message}");
            Console.WriteLine($"Tentativas restantes: {retries}");

            if (retries == 0)
            {
                Console.WriteLine("Falha crítica ao aplicar migrations.");
                throw;
            }

            Thread.Sleep(delay);
        }
    }
}