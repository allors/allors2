namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f278459d-6b7f-47cf-ab0e-05c548faab1e")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]

	[Plural("DistributionChannelRelationships")]
	public partial class DistributionChannelRelationshipClass : Class
	{
		#region Allors
		[Id("86a07419-5dfd-4618-a472-168ba5fdf3ff")]
		[AssociationId("2800f775-ce61-4684-b6a3-5ce28dcf140b")]
		[RoleId("b61fdf73-2420-498c-af0b-49ecdeec359a")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("e7c812db-f6c8-431b-9f4d-5317a0d8673c")]
		[AssociationId("21844f4b-372c-45de-acfa-02c428afdbd8")]
		[RoleId("00b349c4-e7f6-4d8f-b4d3-0922a3465a91")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationClass))]
		[Plural("Distributors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Distributor;



		public static DistributionChannelRelationshipClass Instance {get; internal set;}

		internal DistributionChannelRelationshipClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Distributor.RoleType.IsRequired = true;
            this.InternalOrganisation.RoleType.IsRequired = true;
        }
    }
}