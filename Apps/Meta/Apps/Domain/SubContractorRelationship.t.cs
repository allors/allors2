namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d60cc44a-6491-4982-9b2d-99891e382a21")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]

	[Plural("SubContractorRelationships")]
	public partial class SubContractorRelationshipClass : Class
	{
		#region Allors
		[Id("567a8c58-2584-4dc7-96c8-13fea5b51cf9")]
		[AssociationId("f12711a8-11ce-4f9c-a75b-02594b476a9e")]
		[RoleId("8f21a29f-a0b0-412c-b7dd-e2fcdee561d6")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Contractors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Contractor;

		#region Allors
		[Id("d95ecb34-dfe4-42df-bc9f-1ad4af72abaa")]
		[AssociationId("597810f4-da06-4d63-837e-6cd0419f3d4b")]
		[RoleId("6aca0d56-be58-4876-bfef-918430a119a7")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("SubContractors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SubContractor;



		public static SubContractorRelationshipClass Instance {get; internal set;}

		internal SubContractorRelationshipClass() : base(MetaPopulation.Instance)
        {
        }
	}
}