using GoodHamburger.API.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("CONNECTION STRING:");
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

// Add services
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

// Middleware
app.UseAppSwagger();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Migrations automáticas
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GoodHamburger.Infrastructure.Data.GoodHamburgerContext>();
    dbContext.Database.Migrate();
}

app.Run();