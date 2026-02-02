using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Monq.Core.HealthChecks;

/// <summary>
/// IApplicationBuilder extension methods.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a middleware that provides health check status.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <returns></returns>
    public static IApplicationBuilder UseServicesHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/ready", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains(Constants.TagServicesName)
        });
        return app;
    }
}
