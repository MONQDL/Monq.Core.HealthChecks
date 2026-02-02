using ArangoDB.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Monq.Core.HealthChecks.MonqHealthChecks;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.Extensions;

namespace Monq.Core.HealthChecks;

/// <summary>
/// IHealthChecksBuilder extensions.
/// </summary>
public static class HealthChecksExtensions
{
    /// <summary>
    /// Add RabbitMQ health check using RabbitMQCoreClient library.
    /// </summary>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMonqRabbitMQCheck(this IHealthChecksBuilder healthChecksBuilder) =>
        healthChecksBuilder.AddCheck<RabbitMQCoreClientHealthCheck>("RabbitMQ", tags: new[] { Constants.TagServicesName });

    /// <summary>
    /// Add Redis health check using Monq.Core.Redis library.
    /// </summary>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMonqRedisCheck(this IHealthChecksBuilder healthChecksBuilder,
        IConfiguration configuration)
    {
        // TODO: Use source generator after drop dotnet 7.
        var redisOptions = new RedisOptions();
        configuration.Bind(redisOptions);

        return healthChecksBuilder.Add(new HealthCheckRegistration(
           "Redis",
           sp => new RedisHealthCheck(redisOptions.ToRedisConfig().ToString(includePassword: true)),
           null,
           tags: new string[] { Constants.TagServicesName }));
    }

    /// <summary>
    /// Add EF core Db context health check.
    /// </summary>
    /// <typeparam name="TContext">Context type</typeparam>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMonqDbContextCheck<TContext>(this IHealthChecksBuilder healthChecksBuilder)
        where TContext : DbContext =>
        healthChecksBuilder.AddDbContextCheck<TContext>(tags: new string[] { Constants.TagServicesName });

    /// <summary>
    /// Add ClickHouse health check.
    /// </summary>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMonqClickHouseCheck(this IHealthChecksBuilder healthChecksBuilder,
        string connectionString)
    {
        return healthChecksBuilder.Add(new HealthCheckRegistration(
           "ClickHouse",
           sp => new ClickHouseHealthCheck(connectionString),
           null,
           tags: new string[] { Constants.TagServicesName }));
    }

    /// <summary>
    /// Add ArangoDb health check.
    /// </summary>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMonqArangoDbCheck(this IHealthChecksBuilder healthChecksBuilder,
        DatabaseSharedSetting settings)
    {
        return healthChecksBuilder.Add(new HealthCheckRegistration(
           "ArangoDb",
           sp => new ArangoDbHealthCheck(settings),
           null,
           tags: new string[] { Constants.TagServicesName }));
    }
}
