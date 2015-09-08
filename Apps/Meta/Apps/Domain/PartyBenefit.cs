namespace Allors.Meta
{
    public partial class PartyBenefitClass
	{
	    internal override void AppsExtend()
        {
            this.Employment.RoleType.IsRequired = true;
            this.Benefit.RoleType.IsRequired = true;
        }
	}
}