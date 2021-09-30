namespace Allors.Workspace
{
    public interface IUnitDiff : IDiff
    {
        object OriginalRole { get; }

        object ChangedRole { get; }
    }
}
