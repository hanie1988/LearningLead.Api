namespace Application.Tests.Common;

using Moq;

public abstract class TestingHelper
{
    protected Mock<T> Mock<T>() where T : class
        => new Mock<T>(MockBehavior.Strict);

    protected static CancellationToken Ct => CancellationToken.None;
}