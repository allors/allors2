namespace Application
{
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public interface ITransformation
    {
        object ToExcel(ISessionObject sessionObject, RoleType roleType);

        object ToDomain(dynamic value);
    }
}
