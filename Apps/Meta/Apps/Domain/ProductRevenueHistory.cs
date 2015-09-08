namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2fb4693c-d8c8-49fb-9d99-8ae1a9f43683")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ProductRevenueHistories")]
	public partial class ProductRevenueHistoryClass : Class
	{
		#region Allors
		[Id("c5df5a04-12b0-4b86-87c9-6318bbb05078")]
		[AssociationId("53287e78-440a-4789-a706-a456e5267f0c")]
		[RoleId("94f656f9-e6d0-4fae-b7ee-f6ef5378fd83")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("d7d60054-4b62-428f-a223-0b0852841953")]
		[AssociationId("dd7f995b-7262-466a-b669-789dd5c3b774")]
		[RoleId("cf211c4a-03fd-461b-b0b9-787d7b245ca2")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;

		#region Allors
		[Id("e967f657-6092-473f-9772-552b3372f847")]
		[AssociationId("7af0962e-9d48-4b9f-aed4-80f16c2592c8")]
		[RoleId("678a15bf-f006-439a-8b96-dc8dc6fdd64d")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("ef4c7d18-192b-4880-beb3-645911d6b21a")]
		[AssociationId("b28db2ad-2f30-41b4-8a33-ba1d888c8e2e")]
		[RoleId("af53ba3e-d2cb-4f2c-810b-a3f4c32dbc3f")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;



		public static ProductRevenueHistoryClass Instance {get; internal set;}

		internal ProductRevenueHistoryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}