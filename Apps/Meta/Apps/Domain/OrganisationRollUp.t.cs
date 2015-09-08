namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("316fc0d3-2dce-43aa-9b38-a60f964d5395")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]

	[Plural("OrganisationRollUps")]
	public partial class OrganisationRollUpClass : Class
	{
		#region Allors
		[Id("1ed8bd41-7552-44bd-bcb0-f24c47cf84ca")]
		[AssociationId("924282be-62b0-4a94-814e-04ef94bbeaac")]
		[RoleId("09c50cf9-87b3-4280-80d3-b793b392d168")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationClass))]
		[Plural("Parents")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Parent;

		#region Allors
		[Id("4301bb17-43b6-4bf3-a874-7441dd419dd0")]
		[AssociationId("5b6d83a4-a7f5-4097-bc9b-7ba91e3b96ee")]
		[RoleId("269ea202-bffb-42ff-a497-4a2fa1afbaad")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationUnitClass))]
		[Plural("RollupKinds")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RollupKind;

		#region Allors
		[Id("92ebf310-72ea-468b-a880-7268b48df41a")]
		[AssociationId("71b8ea7b-5316-42df-adc0-2aded71c9eaf")]
		[RoleId("e005418e-9180-4146-b55a-81ff9fb06078")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationClass))]
		[Plural("Children")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Child;



		public static OrganisationRollUpClass Instance {get; internal set;}

		internal OrganisationRollUpClass() : base(MetaPopulation.Instance)
        {
        }
	}
}