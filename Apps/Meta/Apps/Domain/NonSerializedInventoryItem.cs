namespace Allors.Meta
{
    public partial class NonSerializedInventoryItemClass
	{
	    internal override void AppsExtend()
        {
			this.QuantityCommittedOut.RoleType.IsRequired = true;
			this.QuantityOnHand.RoleType.IsRequired = true;
			this.PreviousQuantityOnHand.RoleType.IsRequired = true;
			this.QuantityExpectedIn.RoleType.IsRequired = true;

            this.CurrentObjectState.RoleType.IsRequired = true;
            this.AvailableToPromise.RoleType.IsRequired = true;
        }
	}
}