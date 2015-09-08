namespace Allors.Meta
{
	#region Allors
	[Id("fa4303c8-a09d-4dd5-97b3-76459b8e038d")]
	#endregion
	[Inherit(typeof(RequirementInterface))]
	public partial class WorkRequirementClass : Class
	{
		#region Allors
		[Id("b2d15c8b-a739-4c9d-bc16-eff5e6ca112e")]
		[AssociationId("94c8458e-e890-46b0-bdd4-dbfcb9877ded")]
		[RoleId("22899775-0083-4171-801f-9396c9ba16a3")]
		#endregion
		[Indexed]
		[Type(typeof(FixedAssetInterface))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType FixedAsset;

		#region Allors
		[Id("c9b7298e-1a19-4805-94d6-a6a33acccce0")]
		[AssociationId("664c20b0-6cba-43f8-a52a-2655501b9348")]
		[RoleId("9ae7027e-6541-41fd-bae0-6e61c424c864")]
		#endregion
		[Indexed]
		[Type(typeof(DeliverableClass))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Deliverable;

		#region Allors
		[Id("ef364ba6-62ed-40db-a580-cf7f6f473e27")]
		[AssociationId("beb281bd-199b-4416-bb38-7d21ec376398")]
		[RoleId("8f541554-8bfa-404a-85e9-453f2809d4a4")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;
        
		public static WorkRequirementClass Instance {get; internal set;}

		internal WorkRequirementClass() : base(MetaPopulation.Instance)
        {
        }
	}
}