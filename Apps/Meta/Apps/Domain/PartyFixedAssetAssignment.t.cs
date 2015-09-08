namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("40ee178e-7564-4dfa-ab6f-8bcd4e62b498")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(CommentableInterface))]

	[Plural("PartyFixedAssetAssignments")]
	public partial class PartyFixedAssetAssignmentClass : Class
	{
		#region Allors
		[Id("28afdc0d-ebc7-4f53-b5a1-0cc0eb377887")]
		[AssociationId("8d6a5121-c704-4f04-95de-7e2ab8faecea")]
		[RoleId("e9058932-6beb-4698-89b9-c70e98b30b7f")]
		#endregion
		[Indexed]
		[Type(typeof(FixedAssetInterface))]
		[Plural("FixedAssets")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType FixedAsset;

		#region Allors
		[Id("59187015-4689-4ef8-942f-c36ff4c74e64")]
		[AssociationId("4f0c5035-bfd2-4843-8d6e-d3df15a7f5dd")]
		[RoleId("38f3a7f5-53b5-4572-bcb0-347fa3a543f3")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;

		#region Allors
		[Id("70c38a47-79c4-4ec8-abfd-3c40ef4239ea")]
		[AssociationId("874b5fdc-a8b9-4b7c-9785-15661917b57a")]
		[RoleId("f243ed6d-eabc-4363-ba37-cf147a166081")]
		#endregion
		[Indexed]
		[Type(typeof(AssetAssignmentStatusClass))]
		[Plural("AssetAssignmentStatuses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AssetAssignmentStatus;

		#region Allors
		[Id("c70f014b-345b-48ad-8075-2a1835a19f57")]
		[AssociationId("95b448b4-4fc5-4bd5-b789-e967de001bbe")]
		[RoleId("aa4aca33-b94c-4527-97db-558fab6805a5")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AllocatedCosts")]
		public RelationType AllocatedCost;



		public static PartyFixedAssetAssignmentClass Instance {get; internal set;}

		internal PartyFixedAssetAssignmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}