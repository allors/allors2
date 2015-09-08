namespace Allors.Meta
{
    public partial class ServiceEntryHeaderClass
	{
	    internal override void AppsExtend()
        {
            this.SubmittedDate.RoleType.IsRequired = true;
            this.SubmittedBy.RoleType.IsRequired = true;
        }
	}
}