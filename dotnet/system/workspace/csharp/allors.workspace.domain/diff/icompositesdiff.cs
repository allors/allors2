namespace Allors.Workspace
{
    using System.Collections.Generic;

    public interface ICompositesDiff : IDiff
    {
        IReadOnlyList<IStrategy> OriginalRoles { get; }

        IReadOnlyList<IStrategy> ChangedRoles { get; }
    }
}
