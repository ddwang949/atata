﻿namespace Atata;

[Obsolete("Use " + nameof(AddDirectoryFilesToNUnitTestContextEventHandler) + " instead.")] // Obsolete since v2.11.0.
public class AddDirectoryFilesToNUnitTestContextOnCleanUpEventHandler : IEventHandler<AtataContextCleanUpEvent>
{
    private readonly Func<AtataContext, string> _directoryPathBuilder;

    public AddDirectoryFilesToNUnitTestContextOnCleanUpEventHandler(Func<AtataContext, string> directoryPathBuilder) =>
        _directoryPathBuilder = directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));

    public void Handle(AtataContextCleanUpEvent eventData, AtataContext context)
    {
        string directoryPath = _directoryPathBuilder.Invoke(context);

        directoryPath = context.FillTemplateString(directoryPath);

        DirectoryInfo directory = new DirectoryInfo(directoryPath);

        if (directory.Exists)
        {
            var files = directory.EnumerateFiles("*", SearchOption.AllDirectories)
                .OrderBy(x => x.CreationTimeUtc);

            foreach (var file in files)
                NUnitAdapter.AddTestAttachment(file.FullName);
        }
    }
}
