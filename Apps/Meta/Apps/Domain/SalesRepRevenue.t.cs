namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("749e2a92-b397-4d36-b965-6073d45a4135")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("SalesRepRevenues")]
	public partial class SalesRepRevenueClass : Class
	{
		#region Allors
		[Id("0bf9f020-7704-4e4e-92f6-06e747dc9463")]
		[AssociationId("7ca286bb-a26d-4b7a-bfbe-8305d885d035")]
		[RoleId("7cfa4dad-e2a4-4c95-b25f-476a8a2b7521")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("70b2fc04-ce4e-4af7-b287-02883fe660e9")]
		[AssociationId("48f05073-776f-4465-9763-ca71c785c058")]
		[RoleId("d5009f5b-b990-465d-b868-0bf977b33a4c")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("730e4a76-6af4-441e-8d82-1f3e5807d5a5")]
		[AssociationId("6ad611d9-a2a1-46cc-9ac2-28832723f063")]
		[RoleId("5a92ec5a-4128-46ba-9a75-112592f2662d")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("89ff9736-f2d1-4609-ac99-b60f5b37f406")]
		[AssociationId("86e77422-4347-4e97-99ba-1c3b7cf57220")]
		[RoleId("5c19cc4e-4599-48b7-ab74-a1be3509317d")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("b1aa9e43-5ccc-4e1d-821e-39af02321a79")]
		[AssociationId("092108de-fb58-4fb2-b844-443fd476a383")]
		[RoleId("d1667908-4149-45dd-949e-ab00fbf3c7c4")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("SalesRepNames")]
		public RelationType SalesRepName;

		#region Allors
		[Id("b72d2ab7-ad47-41dd-8dab-4b6364efc342")]
		[AssociationId("6160b809-a3a4-434a-b05a-cfdb1a3a1dd4")]
		[RoleId("6e712c98-649e-49cc-9484-0a0d407f02a7")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("be530b0c-6ab9-43a2-a974-f06015ae3480")]
		[AssociationId("cd786202-3d96-4f6b-94af-27ffb92608e3")]
		[RoleId("0bebaa7b-9332-44a6-abff-454175d2f2a5")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SalesReps")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesRep;



		public static SalesRepRevenueClass Instance {get; internal set;}

		internal SalesRepRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}