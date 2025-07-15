using AspNetCoreRateLimit;

namespace Products.Api.Extensions;

public static class RateLimittingExtensions
{
    public static IServiceCollection AddCustomRateLiming(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache(); // required
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        return services;
    }
}
