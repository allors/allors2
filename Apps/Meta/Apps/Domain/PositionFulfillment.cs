namespace Allors.Meta
{
    public partial class PositionFulfillmentClass
	{
	    internal override void AppsExtend()
        {
            this.Person.RoleType.IsRequired = true;
            this.Position.RoleType.IsRequired = true;
        }
	}
}