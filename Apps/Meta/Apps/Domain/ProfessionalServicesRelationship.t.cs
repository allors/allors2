namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a6f772e6-8f2c-4180-bbf9-2e5ab0f0efc8")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]

	[Plural("ProfessionalServicesRelationships")]
	public partial class ProfessionalServicesRelationshipClass : Class
	{
		#region Allors
		[Id("62edaaeb-bcef-4c3c-955a-30d708bc4a3c")]
		[AssociationId("af3829d6-137c-4453-b705-60b7dfa8c045")]
		[RoleId("29b1fec5-de9c-4fe2-bdfc-fc9d33ca90b5")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Professionals")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Professional;

		#region Allors
		[Id("a587695e-a9b3-4b5b-b211-a19096b88815")]
		[AssociationId("d3fc269c-debf-4ada-b1be-b2f48d2ae027")]
		[RoleId("c6b955f2-20ed-4164-8f11-2c5d24fa0443")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationClass))]
		[Plural("ProfessionalServicesProviders")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProfessionalServicesProvider;



		public static ProfessionalServicesRelationshipClass Instance {get; internal set;}

		internal ProfessionalServicesRelationshipClass() : base(MetaPopulation.Instance)
        {
        }
	}
}