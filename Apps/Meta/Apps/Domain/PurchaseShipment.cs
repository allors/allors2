namespace Allors.Meta
{
    public partial class PurchaseShipmentClass
	{
	    internal override void AppsExtend()
        {
            this.CurrentObjectState.RoleType.IsRequired = true;
            this.Facility.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipToParty.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.EstimatedArrivalDate.RoleType].IsRequiredOverride = true;
        }
	}
}