namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("11d75a7a-2e86-4430-a6af-2916440c9ecb")]
	#endregion
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("JournalEntries")]
	public partial class JournalEntryClass : Class
	{
		#region Allors
		[Id("09202ffd-6b78-455b-a140-a354a771d761")]
		[AssociationId("f4b83ae6-6dc6-495a-8081-a5137434bc7f")]
		[RoleId("0dda1ff5-1420-454b-ad8d-be6e5ee68b91")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("1452d159-857a-4fff-b1d6-6d27772e54bc")]
		[AssociationId("eb122f1d-8615-4342-8beb-2a197677947a")]
		[RoleId("bf8485f3-d5dd-4236-b542-61674c2298db")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("EntryNumbers")]
		public RelationType EntryNumber;

		#region Allors
		[Id("1b5f8acd-872d-498e-9c2d-ded4b7d31efe")]
		[AssociationId("9bb9541a-f0fc-4ed8-bc3f-13e1d7901395")]
		[RoleId("0b9dd5eb-10a1-470b-a119-158c66c558f1")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EntryDates")]
		public RelationType EntryDate;

		#region Allors
		[Id("4eca8284-cc27-4440-8b5f-adeffd3b078b")]
		[AssociationId("b7897efa-b2f5-4807-8385-3da4936998c7")]
		[RoleId("f8bcd82b-5209-45d1-a6ee-5452ca9cf11b")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("JournalDates")]
		public RelationType JournalDate;

		#region Allors
		[Id("e81fe73b-1486-4a9d-ab2b-2d49dfcbb777")]
		[AssociationId("77a3f9d6-814b-438e-b424-e63763bb4213")]
		[RoleId("9afbe2d8-116b-4e35-bbb5-e35085697b30")]
		#endregion
		[Indexed]
		[Type(typeof(JournalEntryDetailClass))]
		[Plural("JournalEntryDetails")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType JournalEntryDetail;



		public static JournalEntryClass Instance {get; internal set;}

		internal JournalEntryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}