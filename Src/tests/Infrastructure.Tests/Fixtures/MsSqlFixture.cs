using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

public class MsSqlFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; } = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("YourStrong@Passw0rd")
        .Build();

    public AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(Container.GetConnectionString())
            .Options;

        return new AppDbContext(options);
    }

    public Task InitializeAsync()
        => Container.StartAsync();

    public Task DisposeAsync()
        => Container.DisposeAsync().AsTask();
}