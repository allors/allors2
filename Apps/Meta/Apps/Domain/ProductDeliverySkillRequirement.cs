namespace Allors.Meta
{
    public partial class ProductDeliverySkillRequirementClass
	{
	    internal override void AppsExtend()
        {
            this.Service.RoleType.IsRequired = true;
            this.Skill.RoleType.IsRequired = true;
        }
	}
}