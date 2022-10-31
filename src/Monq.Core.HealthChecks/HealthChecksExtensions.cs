using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monq.Core.HealthChecks.MonqHealthChecks;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.Extentions;

namespace Monq.Core.HealthChecks
{
    public static class HealthChecksExtensions
    {
        public static IHealthChecksBuilder AddRabbitMQCoreClient(this IHealthChecksBuilder healthChecksBuilder) =>
            healthChecksBuilder.AddCheck<RabbitMQCoreClientHealthCheck>("RabbitMQ", tags: new[] { Constants.TagServicesName });

        public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder healthChecksBuilder, IConfiguration configuration)
        {
            var redisOptions = new RedisOptions();
            configuration.Bind(redisOptions);
            return healthChecksBuilder.AddRedis(redisOptions.ToRedisConfig().ToString(includePassword: true),
                    tags: new string[] { Constants.TagServicesName });
        }
    }
}
