namespace Allors.Meta
{
    public partial class ProductFeatureApplicabilityRelationshipClass
	{
	    internal override void AppsExtend()
        {
            this.AvailableFor.RoleType.IsRequired = true;
            this.UsedToDefine.RoleType.IsRequired = true;
		}
	}
}