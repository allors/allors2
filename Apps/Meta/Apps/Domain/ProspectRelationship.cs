namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d0de3fad-1944-44e5-be99-b74df7a1f917")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]

	[Plural("ProspectRelationships")]
	public partial class ProspectRelationshipClass : Class
	{
		#region Allors
		[Id("6ff817ee-7f62-440b-a699-6a04c0e0b730")]
		[AssociationId("25b90f76-b9ba-4f95-bb17-9479f6ee58e4")]
		[RoleId("5e3e7da0-1c75-4a77-9ac1-0b43b7f4c087")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("e803f51e-a497-4e3a-b082-494fc4af943a")]
		[AssociationId("4e75fb76-3831-4646-99a7-c6b904d867ef")]
		[RoleId("1f4abcc7-8c9a-4e4a-8ff4-48a05c5d4291")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Prospects")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Prospect;



		public static ProspectRelationshipClass Instance {get; internal set;}

		internal ProspectRelationshipClass() : base(MetaPopulation.Instance)
        {
        }
	}
}