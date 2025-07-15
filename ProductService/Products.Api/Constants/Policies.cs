namespace Products.Api.Constants;

/// <summary>
/// Centralized policy names to avoid hardcoding.
/// </summary>
public static class Policies
{
    public const string AdminOnly = "AdminOnly";
    public const string UserOnly = "UserOnly";
    public const string AdminOrManager = "AdminOrManager";
    public const string CanViewReports = "CanViewReports";
}
