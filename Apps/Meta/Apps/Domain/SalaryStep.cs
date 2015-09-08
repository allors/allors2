namespace Allors.Meta
{
    public partial class SalaryStepClass
	{
	    internal override void AppsExtend()
        {
            this.ModifiedDate.RoleType.IsRequired = true;
            this.Amount.RoleType.IsRequired = true;
        }
	}
}