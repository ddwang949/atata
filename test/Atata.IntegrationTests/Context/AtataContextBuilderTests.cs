﻿namespace Atata.IntegrationTests.Context;

public sealed class AtataContextBuilderTests : TestSuiteBase
{
    [Test]
    public void UseParentContext()
    {
        // Arrange
        using var parentContext = AtataContext.CreateBuilder(AtataContextScope.TestSuite)
            .Build();

        // Act
        using var childContext1 = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseParentContext(parentContext)
            .Build();

        using var childContext2 = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseParentContext(parentContext)
            .Build();

        // Assert
        parentContext.AggregateAssert(() =>
        {
            parentContext.ToSubject().ValueOf(x => x.ChildContexts).Should.EqualSequence(childContext1, childContext2);
            childContext1.ToSubject().ValueOf(x => x.ParentContext).Should.Be(parentContext);
            childContext2.ToSubject().ValueOf(x => x.ParentContext).Should.Be(parentContext);
        });
    }

    [Test]
    public void WhenThrowsOnBuild()
    {
        var builder = ConfigureSessionlessAtataContext();
        builder.EventSubscriptions.Add<AtataContextInitCompletedEvent>(
            () => throw new InvalidOperationException());

        Assert.That(
            () => builder.Build(),
            Throws.TypeOf<InvalidOperationException>());

        CurrentLog.GetSnapshotOfLevel(LogLevel.Error).
            Should().ContainSingle();
        CurrentLog.LatestRecord.Level.Should().Be(LogLevel.Debug);
        CurrentLog.LatestRecord.Message.Should().StartWith("Finished");
    }
}
