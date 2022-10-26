using Microsoft.Extensions.DependencyInjection;
using Monq.Core.HealthChecks.MonqHealthChecks;

namespace Monq.Core.HealthChecks
{
    public static class HealthChecksExtensions
    {
        public static IHealthChecksBuilder AddRabbitMQCoreClient(this IHealthChecksBuilder healthChecksBuilder) =>
            healthChecksBuilder.AddCheck<RabbitMQCoreClientHealthCheck>("RabbitMQ", tags: new[] { "services" });
    }
}
