﻿namespace Atata;

[Obsolete("Use " + nameof(TakePageSnapshotOnNUnitErrorEventHandler) + " instead.")] // Obsolete since v2.11.0.
public class TakePageSnapshotOnNUnitErrorOnCleanUpEventHandler : IConditionalEventHandler<AtataContextCleanUpEvent>
{
    private readonly string _snapshotTitle;

    public TakePageSnapshotOnNUnitErrorOnCleanUpEventHandler(string snapshotTitle = "Failed") =>
        _snapshotTitle = snapshotTitle;

    public bool CanHandle(AtataContextCleanUpEvent eventData, AtataContext context) =>
        NUnitAdapter.IsCurrentTestFailed() && context.HasDriver;

    public void Handle(AtataContextCleanUpEvent eventData, AtataContext context) =>
        context.TakePageSnapshot(_snapshotTitle);
}
