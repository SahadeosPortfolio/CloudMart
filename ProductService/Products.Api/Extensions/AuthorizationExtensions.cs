using Products.Api.Constants;

namespace Products.Api.Extensions;

/// <summary>
/// Extension methods for configuring authorization policies.
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Adds custom authorization policies to the service collection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Role-based policies
            options.AddPolicy(Policies.AdminOnly, policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy(Policies.UserOnly, policy =>
                policy.RequireRole("User"));

            // Multiple-role access
            options.AddPolicy(Policies.AdminOrManager, policy =>
                policy.RequireRole("Admin", "Manager"));

            // Claim-based access (example)
            options.AddPolicy(Policies.CanViewReports, policy =>
                policy.RequireClaim("permission", "report.view"));

            // You can add more granular policies here
        });

        return services;
    }
}