namespace Allors.Meta
{
    public partial class ProfessionalServicesRelationshipClass
	{
	    internal override void AppsExtend()
        {
            this.Professional.RoleType.IsRequired = true;
            this.ProfessionalServicesProvider.RoleType.IsRequired = true;
        }
	}
}