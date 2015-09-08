namespace Allors.Meta
{
    public partial class PartBillOfMaterialSubstituteClass
	{
	    internal override void AppsExtend()
        {
            this.PartBillOfMaterial.RoleType.IsRequired = true;
            this.SubstitutionPartBillOfMaterial.RoleType.IsRequired = true;
        }
	}
}