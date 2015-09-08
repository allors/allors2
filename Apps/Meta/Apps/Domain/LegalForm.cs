namespace Allors.Meta
{
    public partial class LegalFormClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}