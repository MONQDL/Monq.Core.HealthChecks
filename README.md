# Monq Health checks library for .net core applications with Dependency Injection support

The library integrates the API point /ready that triggers the configured health checks and 
responses http 200 on healthy status or http 503 on unhealthy status.

## Installation

```powershell
Install-Package Monq.Core.HealthChecks
```

## Using the library

In the ASP.NET Core project add health checks at `Service Collection` configuration stage.

```csharp
services.AddHealthChecks()
  .AddMonqRabbitMQCheck()
  .AddMonqRedisCheck(Configuration.GetSection(RedisSection))
  .AddMonqDbContextCheck<FmProjectsContext>()
  .AddMonqClickHouseCheck(Configuration[ClickHouseConnectionString]);
```

Configure `HealthChecks` middleware at `app middleware` configuration stage after the `app.UseRouting()`.

```csharp
app.UseRouting();
app.UseServicesHealthChecks();
```

### Health Cheks

`AddMonqRabbitMQCheck()` - adds check of the `RabbitMQCoreClient` library was configured properly.

`AddMonqRedisCheck()` - adds check of the `Monq.Core.Redis` library was configured properly.

`AddMonqDbContextCheck<T>()` - adds check of the `DbContext` was configured properly.

`AddMonqClickHouseCheck()` - adds check of the `ClickHouse.Client` library was configured properly.

`AddMonqArangoDbCheck()` - adds check of the `ArangoDB.Client` library was configured properly.
