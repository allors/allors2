namespace Allors.Meta
{
    public partial class PartyFixedAssetAssignmentClass
	{
	    internal override void AppsExtend()
        {
            this.FixedAsset.RoleType.IsRequired = true;
            this.Party.RoleType.IsRequired = true;
        }
	}
}