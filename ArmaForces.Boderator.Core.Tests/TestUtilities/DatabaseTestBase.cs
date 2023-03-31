using System;
using System.Transactions;
using ArmaForces.Boderator.Core.DependencyInjection;
using ArmaForces.Boderator.Core.Tests.Features.Missions.Helpers;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ArmaForces.Boderator.Core.Tests.TestUtilities;

public class DatabaseTestBase : IDisposable
{
    protected readonly IServiceProvider ServiceProvider = new ServiceCollection()
        .AddBoderatorCore(_ => TestDatabaseConstants.TestDbConnectionString)
        .AddScoped<MissionsDbHelper>()
        .AddScoped<SignupsDbHelper>()
        .BuildServiceProvider();

    // protected IDbContextTransaction? DbContextTransaction { get; init; }
    private TransactionScope? TransactionScope { get; }

    protected DatabaseTestBase()
    {
        TransactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public void Dispose()
    {
        TransactionScope?.Dispose();
        // DbContextTransaction?.Rollback();
        // DbContextTransaction?.Dispose();
    }
}
