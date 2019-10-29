namespace Allors.Workspace.Blazor.Bootstrap.Forms.Roles
{
    public class ABSCheckboxBase : RoleField
    {
        public bool BoolModel
        {
            get => (bool?)this.Model ?? false;
            set => this.Model = value;
        }
    }
}
