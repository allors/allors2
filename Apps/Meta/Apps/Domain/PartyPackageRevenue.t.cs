namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("96fe3000-606e-4f88-ba04-87544ef176ca")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("PartyPackageRevenues")]
	public partial class PartyPackageRevenueClass : Class
	{
		#region Allors
		[Id("3fc82b94-ce74-42d7-91d8-e97a79117f4f")]
		[AssociationId("917ecd65-8097-4e6b-93ce-662b18ccf424")]
		[RoleId("33bf1c47-0d26-43e4-841d-bb5d85da1bdf")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Months")]
		public RelationType Month;

		#region Allors
		[Id("646382fa-3794-46be-81a0-28a1609e65b0")]
		[AssociationId("0bb2b710-90c3-42c9-8eb4-5bffb06cb705")]
		[RoleId("0ec8beac-06c9-4f7f-a39b-fb9d7bfcae0f")]
		#endregion
		[Indexed]
		[Type(typeof(PackageClass))]
		[Plural("Packages")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Package;

		#region Allors
		[Id("8896e95d-80e3-42dc-8ba7-ad3fdef665f9")]
		[AssociationId("5d061264-d9c8-471c-b5be-3251502e24e1")]
		[RoleId("d1757181-9a09-4075-99ac-5c2a13ad85d3")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;

		#region Allors
		[Id("af3eeba0-867c-4484-b593-1815b38c8bf4")]
		[AssociationId("450fc3e5-f6ca-4f96-ab52-afb71421b6b5")]
		[RoleId("80615a8d-b2ff-4671-b9bb-0667413cd74c")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("b9648327-4521-4daf-b68f-52cd78095998")]
		[AssociationId("e65e0fb0-26a8-4640-ae6a-c8402889dc8e")]
		[RoleId("b3788737-41da-4480-8223-bc398e021561")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Years")]
		public RelationType Year;

		#region Allors
		[Id("e8384ea1-cb9d-43a0-8409-68dc86ca8def")]
		[AssociationId("f50f2ba1-9d0b-4eee-87d2-626fd89422c7")]
		[RoleId("bf38c186-30a1-40e5-90eb-a326865a2d19")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("PartyPackageNames")]
		public RelationType PartyPackageName;

		#region Allors
		[Id("e93042c4-1e6c-47c5-9004-f6ddcbfdbb33")]
		[AssociationId("39c71cdb-847a-474f-92b4-827e2eb95c22")]
		[RoleId("d498a399-c183-433c-8260-396c4e2b997d")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("ed2b5e8c-2c74-4ae2-b467-d41baf9f41db")]
		[AssociationId("1b51addf-6a5d-4611-b23c-1a16fa413259")]
		[RoleId("2734ebff-1038-4945-98c5-d1da0a11265b")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;



		public static PartyPackageRevenueClass Instance {get; internal set;}

		internal PartyPackageRevenueClass() : base(MetaPopulation.Instance)
        {
        }
	}
}