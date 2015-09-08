namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("734be1c9-e6af-49b7-8fe8-331cd7036e2e")]
	#endregion
	[Plural("AgreementTerms")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class AgreementTermInterface: Interface
	{
		#region Allors
		[Id("85cd1bbd-f2ad-454f-8f04-cdea48ce6196")]
		[AssociationId("c28f6487-83a7-49b3-911d-21f691ae7d02")]
		[RoleId("7eb5e9b4-834f-4774-ac40-11bd455a6ea8")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("TermValues")]
		public RelationType TermValue;

		#region Allors
		[Id("b38a35f7-3bf5-4c9c-b2ea-6b220de43e20")]
		[AssociationId("a2786ad1-c4b0-4394-8841-ad14de467bc4")]
		[RoleId("7e1163db-78d1-4c63-b10a-d1315ccb223c")]
		#endregion
		[Indexed]
		[Type(typeof(TermTypeClass))]
		[Plural("TermTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TermType;

		#region Allors
		[Id("d9a68cc0-8fea-4610-9853-f1fca33cbc9a")]
		[AssociationId("35b4a0af-89c6-44e0-981f-439b632a6d51")]
		[RoleId("9b734740-dc58-428e-8031-de5341e5aae7")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static AgreementTermInterface Instance {get; internal set;}

		internal AgreementTermInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}