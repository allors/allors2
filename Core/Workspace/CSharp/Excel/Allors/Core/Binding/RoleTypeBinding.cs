namespace Allors.Excel
{
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Dipu.Excel;

    public class RoleTypeBinding : IBinding
    {
        public ISessionObject Object { get; }

        public RoleType RoleType { get; }

        public bool OneWayBinding { get; }

        public bool TwoWayBinding => !this.OneWayBinding;

        public RoleTypeBinding(ISessionObject @object, RoleType roleType, bool oneWayBinding = false)
        {
            Object = @object;
            RoleType = roleType;
            this.OneWayBinding = oneWayBinding;
        }

        public void ToCell(ICell cell)
        {
            cell.Value = this.Object.Get(this.RoleType);
        }

        public void ToDomain(ICell cell)
        {
            if (this.TwoWayBinding)
            {
                this.Object.Set(this.RoleType, cell.Value);
            }
        }
    }
}
