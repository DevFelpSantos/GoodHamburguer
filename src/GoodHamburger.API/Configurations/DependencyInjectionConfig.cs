namespace GoodHamburger.API.Configurations
{
    using FluentValidation;
    using GoodHamburger.Application.DTOs;
    using GoodHamburger.Application.Services;
    using GoodHamburger.Application.Services.Interfaces;
    using GoodHamburger.Application.Validators;
    using GoodHamburger.Domain.Interfaces;
    using GoodHamburger.Infrastructure.Data;
    using GoodHamburger.Infrastructure.Repositories;
    using Microsoft.EntityFrameworkCore;

    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GoodHamburgerContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            services.AddScoped<ICardapioService, CardapioService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IDescontoService, DescontoService>();

            services.AddScoped<IValidator<CriarPedidoRequestDto>, CriarPedidoValidator>();

            return services;
        }
    }
}