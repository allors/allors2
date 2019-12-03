namespace Allors.Excel
{
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Dipu.Excel;

    public class RoleTypeBinding : IBinding
    {
        public ISessionObject Object { get; }

        public RoleType RoleType { get; }

        public RoleTypeBinding(ISessionObject @object, RoleType roleType)
        {
            Object = @object;
            RoleType = roleType;
        }

        public void ToCell(ICell cell)
        {
            cell.Value = this.Object.Get(this.RoleType);
        }

        public void ToDomain(ICell cell)
        {
            this.Object.Set(this.RoleType, cell.Value);
        }
    }
}
