namespace Allors.Meta
{
	public partial class StoreClass
	{
	    internal override void AppsExtend()
        {
			this.SalesOrderCounter.RoleType.IsRequired = true;
            this.OutgoingShipmentCounter.RoleType.IsRequired = true;

            this.Name.RoleType.IsRequired = true;
            this.PaymentNetDays.RoleType.IsRequired = true;
            this.PaymentGracePeriod.RoleType.IsRequired = true;
            this.CreditLimit.RoleType.IsRequired = true;
            this.ShipmentThreshold.RoleType.IsRequired = true;
            this.OrderThreshold.RoleType.IsRequired = true;

            this.DefaultPaymentMethod.RoleType.IsRequired = true;
            this.DefaultShipmentMethod.RoleType.IsRequired = true;
            this.DefaultCarrier.RoleType.IsRequired = true;
		}
	}
}