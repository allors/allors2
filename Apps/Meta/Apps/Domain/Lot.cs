namespace Allors.Meta
{
    public partial class LotClass
	{
	    internal override void AppsExtend()
        {
            this.LotNumber.RoleType.IsRequired = true;
        }
	}
}