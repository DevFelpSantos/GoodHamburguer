namespace GoodHamburger.API.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddAppSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Good Hamburger API",
                    Version = "v1",
                    Description = "API para gerenciamento de pedidos de hambúrgueres"
                });
            });

            return services;
        }

        public static WebApplication UseAppSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Good Hamburger API v1");
                });
            }

            return app;
        }
    }
}