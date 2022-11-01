using ClickHouse.Client.ADO;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monq.Core.HealthChecks.MonqHealthChecks
{
    internal sealed class ClickHouseHealthCheck : IHealthCheck
    {
        readonly string _connectionString;
        public ClickHouseHealthCheck(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));

            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            ClickHouseConnection? connection = null;
            try
            {
                connection = new ClickHouseConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);
                return HealthCheckResult.Healthy("A healthy result.");
            }
            catch (Exception e)
            {
                return 
                    HealthCheckResult.Unhealthy("An unhealthy result.", exception: e);
            }
            finally
            {
                connection?.Close();
                connection?.Dispose();
            }
        }
    }
}
