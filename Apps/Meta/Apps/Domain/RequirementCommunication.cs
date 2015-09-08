namespace Allors.Meta
{
    public partial class RequirementCommunicationClass
	{
	    internal override void AppsExtend()
        {
            this.AssociatedProfessional.RoleType.IsRequired = true;
            this.CommunicationEvent.RoleType.IsRequired = true;
            this.Requirement.RoleType.IsRequired = true;
        }
	}
}