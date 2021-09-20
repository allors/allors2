namespace Allors.Workspace
{
    using Meta;

    public interface IDiff
    {
        IRelationType RelationType { get; }

        IStrategy Association { get; }
    }
}
