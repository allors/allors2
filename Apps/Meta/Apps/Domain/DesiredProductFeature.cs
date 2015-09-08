namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("dda88fe9-14b3-463b-ae66-25dd1b136e16")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("DesiredProductFeatures")]
	public partial class DesiredProductFeatureClass : Class
	{
		#region Allors
		[Id("24695d7b-5c61-4b5c-be90-0f18ca46c6a6")]
		[AssociationId("c0720b85-3e00-4ad7-8a19-9c6761aa1bba")]
		[RoleId("360db95e-a5ad-4771-ad23-d591be1640d2")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("Requireds")]
		public RelationType Required;

		#region Allors
		[Id("d09dbd42-5c59-4d78-b5d7-4dbee0406ead")]
		[AssociationId("4a611e8a-1bd5-42b3-b2ba-14403328696b")]
		[RoleId("93f73ab2-af4a-47d1-822b-df576f5a5e86")]
		#endregion
		[Indexed]
		[Type(typeof(ProductFeatureInterface))]
		[Plural("ProductFeatures")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductFeature;



		public static DesiredProductFeatureClass Instance {get; internal set;}

		internal DesiredProductFeatureClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Required.RoleType.IsRequired = true;
            this.ProductFeature.RoleType.IsRequired = true;
        }
    }
}