namespace Allors.Workspace
{
    public interface ICompositeDiff : IDiff
    {
        IStrategy OriginalRole { get; }

        IStrategy ChangedRole { get; }
    }
}
