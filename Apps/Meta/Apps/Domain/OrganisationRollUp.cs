namespace Allors.Meta
{
    public partial class OrganisationRollUpClass
	{
	    internal override void AppsExtend()
        {
            this.Child.RoleType.IsRequired = true;
            this.Parent.RoleType.IsRequired = true;
            this.RollupKind.RoleType.IsRequired = true;
        }
	}
}