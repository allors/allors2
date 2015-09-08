namespace Allors.Meta
{
	#region Allors
	[Id("f77787aa-66af-4d6a-bbe1-ce3d93020185")]
	#endregion
	[Inherit(typeof(ServiceEntryInterface))]
	public partial class MaterialsUsageClass : Class
	{
		#region Allors
		[Id("a244ab38-6469-4aa4-ae7e-c245f17f2368")]
		[AssociationId("719acc0e-aaa9-465a-a08a-a283635cf48c")]
		[RoleId("441feb11-9913-4c2d-a27f-01f0c4ed27ae")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;

		public static MaterialsUsageClass Instance {get; internal set;}

		internal MaterialsUsageClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Amount.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ServiceEntryInterface.Instance.Description.RoleType].IsRequiredOverride = true;
        }
    }
}