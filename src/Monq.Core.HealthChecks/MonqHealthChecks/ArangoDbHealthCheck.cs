using ArangoDB.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monq.Core.HealthChecks.MonqHealthChecks;

internal sealed class ArangoDbHealthCheck : IHealthCheck
{
    const string ArangoDbSettingsIdentifier = "ArangoDbHealthCheck";
    public ArangoDbHealthCheck(DatabaseSharedSetting settings)
    {
        if (settings == null)
            throw new ArgumentNullException(nameof(settings), $"{nameof(settings)} is null.");

        ArangoDatabase.ChangeSetting(ArangoDbSettingsIdentifier, opt =>
        {
            opt.ClusterMode = settings.ClusterMode;
            opt.Collection = settings.Collection;
            opt.Credential = settings.Credential;
            opt.Cursor = settings.Cursor;
            opt.Database = settings.Database;
            opt.DisableChangeTracking = settings.DisableChangeTracking;
            opt.Document = settings.Document;
            opt.Linq = settings.Linq;
            opt.Logger = settings.Logger;
            opt.Serialization = settings.Serialization;
            opt.SystemDatabaseCredential = settings.SystemDatabaseCredential;
            opt.ThrowForServerErrors = settings.ThrowForServerErrors;
            opt.Url = settings.Url;
            opt.WaitForSync = settings.WaitForSync;
        });
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        IArangoDatabase? connection = null;
        try
        {
            connection = ArangoDatabase.CreateWithSetting(ArangoDbSettingsIdentifier);
            await connection.CurrentDatabaseInformationAsync();
            return HealthCheckResult.Healthy("A healthy result.");
        }
        catch (Exception e)
        {
            return
                HealthCheckResult.Unhealthy("An unhealthy result.", exception: e);
        }
        finally
        {
            connection?.Dispose();
        }
    }
}
