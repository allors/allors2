namespace Allors.Workspace.Blazor.Ant.Forms.Roles
{
    public class ACheckboxBase : RoleField
    {
        public bool BoolModel
        {
            get => (bool?)this.Model ?? false;
            set => this.Model = value;
        }
    }
}
