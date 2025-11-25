using Xunit;

public abstract class InfrastructureTestBase : IClassFixture<MsSqlFixture>
{
    protected readonly MsSqlFixture Fixture;

    protected InfrastructureTestBase(MsSqlFixture fixture)
    {
        Fixture = fixture;
    }
}