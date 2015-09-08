namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b55d1ad5-0ef0-40f0-b8d4-b39c370d7dcf")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]

	[Plural("Partnerships")]
	public partial class PartnershipClass : Class
	{
		#region Allors
		[Id("c8eafc73-9fb3-4a7b-8349-1dd1e9f64520")]
		[AssociationId("4d6ee3e0-4c0c-4387-b140-e2296c8bcbd4")]
		[RoleId("386770df-4089-482e-9b54-af375c37319f")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("c9a60b88-e525-4bcd-94bd-3fca8989319f")]
		[AssociationId("309ffb3e-7cd3-4958-9177-e7f25a272579")]
		[RoleId("f77b776b-b957-418b-acfb-a4aad51f7a8a")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationClass))]
		[Plural("Partners")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Partner;



		public static PartnershipClass Instance {get; internal set;}

		internal PartnershipClass() : base(MetaPopulation.Instance)
        {
        }
	}
}