namespace Products.Api.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCustomCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("https://yourfrontend.com")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });

        return services;
    }
}
