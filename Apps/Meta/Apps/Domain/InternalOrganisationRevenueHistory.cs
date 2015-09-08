namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("684ce40f-6950-4163-b110-e83e65c31f0a")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("InternalOrganisationRevenueHistories")]
	public partial class InternalOrganisationRevenueHistoryClass : Class
	{
		#region Allors
		[Id("22b47e4a-5657-4bf6-acff-d923ef5ef8e2")]
		[AssociationId("4f48cd93-af57-4e7a-b54b-3aef374444e7")]
		[RoleId("2aea8ef1-d353-43e4-8f1a-09b2052603e2")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("4fe311fd-b793-43ce-b3de-c6606ea53b34")]
		[AssociationId("08a439a9-cb0d-4910-818f-11b9819bff86")]
		[RoleId("383ed00b-7c75-40cb-b0d7-2bcc6b9f5e62")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AllorsDecimals")]
		public RelationType AllorsDecimal;

		#region Allors
		[Id("a8ab7445-38b2-4e1b-bee2-15d1a91fc239")]
		[AssociationId("cc5041b1-da31-472d-ab66-105561bcb2de")]
		[RoleId("1d88f2e2-6a81-42f8-8e60-242918c82e21")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("b32232d6-6a5b-438b-bfb5-a7495335f9c9")]
		[AssociationId("14c55588-7e1f-4ce2-a080-f14592052442")]
		[RoleId("57afc02e-c13e-41fa-a1cd-0dfe2358649b")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;



		public static InternalOrganisationRevenueHistoryClass Instance {get; internal set;}

		internal InternalOrganisationRevenueHistoryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}