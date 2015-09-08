namespace Allors.Meta
{
	#region Allors
	[Id("7d2d9452-f250-47c3-81e0-4e1c0655cc86")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	public partial class WorkEffortTypeClass : Class
	{
		#region Allors
		[Id("5ce1a600-62a9-4d2c-bfb5-bfe374b2099f")]
		[AssociationId("4d892148-3583-46a2-b68d-895274b9ea7a")]
		[RoleId("fa8657ae-5132-4f37-aba0-4f95c4b1df1e")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortFixedAssetStandardClass))]
		[Plural("WorkEffortFixedAssetStandards")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType WorkEffortFixedAssetStandard;

		#region Allors
		[Id("776839ee-f6cb-4334-a017-4ffdfddd152a")]
		[AssociationId("db3c9fba-5ca8-4296-b1a2-d306ad42dbcc")]
		[RoleId("764e51c4-8a6f-403d-849a-1bf3a1a64911")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortGoodStandardClass))]
		[Plural("WorkEffortGoodStandards")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType WorkEffortGoodStandard;

		#region Allors
		[Id("89eef4e3-eda7-4336-91cb-ce7a7e96521f")]
		[AssociationId("4b61f74f-db7c-4733-b6f5-d485e432a16e")]
		[RoleId("8b9f019b-e79e-4282-9ee1-9bc652fd6817")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortTypeClass))]
		[Plural("Children")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Child;

		#region Allors
		[Id("8d9f51b5-2c8d-4a25-a45e-c79542a09434")]
		[AssociationId("687e9909-0efa-4a04-b705-96d93547458a")]
		[RoleId("da4648c1-b495-4555-8fa3-c4ba8141e67d")]
		#endregion
		[Indexed]
		[Type(typeof(FixedAssetInterface))]
		[Plural("FixedAssetsToRepair")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType FixedAssetToRepair;

		#region Allors
		[Id("93cfed3d-ae24-4a07-becf-34cdc3cdef3e")]
		[AssociationId("958d3cdc-0dbe-4ce6-81c7-492825727ada")]
		[RoleId("d0fadb8a-4891-4caf-a010-6197676cfd54")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("b6d68eff-8a3a-473f-bb4e-9bc46808bde0")]
		[AssociationId("1996225b-a372-44ad-b00a-b257a355d756")]
		[RoleId("2c704b66-922a-4c5c-81fb-973545230501")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortTypeClass))]
		[Plural("Dependencies")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Dependency;

		#region Allors
		[Id("ccf22455-c42a-4f9c-8975-813431bcdd8b")]
		[AssociationId("f70821d5-2a7f-4fdd-ae45-b8c7966710fc")]
		[RoleId("abe54968-9a36-43c5-a57c-b8d1cde032ea")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortTypeKindClass))]
		[Plural("WorkEffortTypeKinds")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType WorkEffortTypeKind;

		#region Allors
		[Id("d51d620e-250e-4492-8926-c8535fad19ec")]
		[AssociationId("e26db451-eb86-44b1-b3cb-eb29d4311157")]
		[RoleId("2a4de99b-9544-4c67-b936-431622654f09")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortPartStandardClass))]
		[Plural("WorkEffortPartStandards")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType WorkEffortPartStandard;

		#region Allors
		[Id("df104ec4-6247-4199-bce1-635978fa8ad4")]
		[AssociationId("0826559c-11ef-4075-ad8e-28c7ed693f1c")]
		[RoleId("073f09b7-1502-4bac-b344-76f4cb6f3907")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortSkillStandardClass))]
		[Plural("WorkEffortSkillStandards")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType WorkEffortSkillStandard;

		#region Allors
		[Id("df1fa89e-25e2-4b72-a928-67fa2c95ad70")]
		[AssociationId("361d313b-8313-43bd-8a98-9b2516ca25f7")]
		[RoleId("3d4d3fd1-28dd-4349-bb57-b869687f5f82")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("StandardWorkHoursPlural")]
		public RelationType StandardWorkHours;

		#region Allors
		[Id("ee521062-a2bf-4a7f-80e4-8da6f63439fe")]
		[AssociationId("fc63a85e-7bc4-49ec-89f5-66fef934f11a")]
		[RoleId("5d9c847c-f2a5-4353-8558-880f60e75925")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("ProductsToProduce")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductToProduce;

		#region Allors
		[Id("f8766ab1-b0ed-42fa-806c-c40a2e68d72a")]
		[AssociationId("7e7a9632-76a8-48c3-ada3-fcc3aa06a511")]
		[RoleId("9a3b5dd0-a399-43ef-9607-32136bb5f3cd")]
		#endregion
		[Indexed]
		[Type(typeof(DeliverableClass))]
		[Plural("DeliverablesToProduce")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DeliverableToProduce;

		public static WorkEffortTypeClass Instance {get; internal set;}

		internal WorkEffortTypeClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.WorkEffortTypeKind.RoleType.IsRequired = true;
        }
    }
}