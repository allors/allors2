namespace Allors.Meta
{
    public partial class TelecommunicationsNumberClass
	{
	    internal override void AppsExtend()
        {
            this.ContactNumber.RoleType.IsRequired = true;
        }
	}
}