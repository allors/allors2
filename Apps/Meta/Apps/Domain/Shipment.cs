namespace Allors.Meta
{
    public partial class ShipmentInterface
	{
	    internal override void AppsExtend()
        {
            this.ShipmentNumber.RoleType.IsRequired = true;
            this.ShipToParty.RoleType.IsRequired = true;
            this.ShipFromParty.RoleType.IsRequired = true;
        }
	}
}