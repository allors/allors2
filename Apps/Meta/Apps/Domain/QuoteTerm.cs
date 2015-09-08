namespace Allors.Meta
{
    public partial class QuoteTermClass
	{
	    internal override void AppsExtend()
        {
            this.TermType.RoleType.IsRequired = true;
        }
	}
}