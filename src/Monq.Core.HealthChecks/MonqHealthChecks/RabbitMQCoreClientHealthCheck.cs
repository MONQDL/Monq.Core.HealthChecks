using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQCoreClient;
using System.Threading;
using System.Threading.Tasks;

namespace Monq.Core.HealthChecks.MonqHealthChecks
{
    public class RabbitMQCoreClientHealthCheck : IHealthCheck
    {
        readonly IQueueService _queueService;

        public RabbitMQCoreClientHealthCheck(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthCheckResultHealthy = _queueService.Connection.IsOpen;

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}
