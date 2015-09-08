namespace Allors.Meta
{
    public partial class PartSpecificationStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.PartSpecificationObjectState.RoleType.IsRequired = true;
        }
	}
}