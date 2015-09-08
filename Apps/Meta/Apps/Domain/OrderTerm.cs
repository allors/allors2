namespace Allors.Meta
{
    public partial class OrderTermClass
	{
	    internal override void AppsExtend()
        {
            this.TermType.RoleType.IsRequired = true;
        }
	}
}