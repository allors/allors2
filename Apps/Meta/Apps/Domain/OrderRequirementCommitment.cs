namespace Allors.Meta
{
    public partial class OrderRequirementCommitmentClass
	{
	    internal override void AppsExtend()
        {
            this.Quantity.RoleType.IsRequired = true;
            this.OrderItem.RoleType.IsRequired = true;
            this.Requirement.RoleType.IsRequired = true;
        }
	}
}