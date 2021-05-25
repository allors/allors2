using Allors.Workspace;
using Allors.Workspace.Meta;

namespace Application
{
    public interface ITransformation
    {
        object ToExcel(ISessionObject sessionObject, RoleType roleType);

        object ToDomain(dynamic value);
    }
}
