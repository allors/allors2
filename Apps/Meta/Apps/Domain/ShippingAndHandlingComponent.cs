namespace Allors.Meta
{
    public partial class ShippingAndHandlingComponentClass
	{
	    internal override void AppsExtend()
        {
            this.SpecifiedFor.RoleType.IsRequired = true;
        }
	}
}