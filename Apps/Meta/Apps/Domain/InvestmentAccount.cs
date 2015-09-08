namespace Allors.Meta
{
    public partial class InvestmentAccountClass
	{
	    internal override void AppsExtend()
	    {
	        this.Name.RoleType.IsRequired = true;
	    }
	}
}