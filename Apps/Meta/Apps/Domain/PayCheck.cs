namespace Allors.Meta
{
    public partial class PayCheckClass
	{
	    internal override void AppsExtend()
        {
            this.Employment.RoleType.IsRequired = true;
        }
	}
}