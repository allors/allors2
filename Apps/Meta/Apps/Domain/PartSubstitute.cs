namespace Allors.Meta
{
    public partial class PartSubstituteClass
	{
	    internal override void AppsExtend()
        {
            this.Part.RoleType.IsRequired = true;
            this.SubstitutionPart.RoleType.IsRequired = true;
            this.Quantity.RoleType.IsRequired = true;
        }
	}
}