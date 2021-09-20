namespace Allors.Workspace.Blazor.Ant.Forms.Roles
{
    public class ANumberInputBase : RoleField
    {
        public int? IntModel { get => (int?)this.Model; set => this.Model = value; }
    }
}
