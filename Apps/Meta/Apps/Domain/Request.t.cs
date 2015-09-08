namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("321a6047-2233-4bec-a1b1-9b965c0099e5")]
	#endregion
	[Plural("Requests")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]

  	public partial class RequestInterface: Interface
	{
		#region Allors
		[Id("1bb3a4b8-224a-47ab-b05b-c0c8a87ec09c")]
		[AssociationId("57109e48-b116-4ea5-b636-73816c0dda68")]
		[RoleId("d63a2e09-95e1-4c90-83a1-a5366a3d5ca3")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("208f711f-5d9d-4dc3-89ad-b1583ad06582")]
		[AssociationId("d91ef645-f5ef-4f09-9d6b-c023d02978f5")]
		[RoleId("c1467dbc-9b64-49a0-8715-90ad277b02c9")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("RequiredResponseDates")]
		public RelationType RequiredResponseDate;

		#region Allors
		[Id("25332874-3ec6-41d8-ac6a-77dd4328e503")]
		[AssociationId("acae3045-3612-4cac-9994-ca81d350da74")]
		[RoleId("576e5797-b3d3-41ab-a788-2b3eeba36f18")]
		#endregion
		[Indexed]
		[Type(typeof(RequestItemClass))]
		[Plural("RequestItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType RequestItem;

		#region Allors
		[Id("8ac90ec6-9d3e-45fe-aaba-27d0c1c058a1")]
		[AssociationId("438f6e6a-292b-4579-bf87-7478c48b9159")]
		[RoleId("c16a1509-cfd6-4f9d-87ce-4b903365b9e5")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("RequestNumbers")]
		public RelationType RequestNumber;

		#region Allors
		[Id("c3389cec-ee8e-45e2-a1eb-01c9a87b2df0")]
		[AssociationId("b5aaad5b-568c-405d-9018-3ff0fcde7dd2")]
		[RoleId("934585ce-6dc2-46cd-a227-24a1cb85fa60")]
		#endregion
		[Indexed]
		[Type(typeof(RespondingPartyClass))]
		[Plural("RespondingParties")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType RespondingParty;

		#region Allors
		[Id("f1a50d9d-2e79-45ac-8f23-8f38fab985c1")]
		[AssociationId("fe8fd88b-8b7d-4998-bf59-56b4e8d44571")]
		[RoleId("2e871e31-a702-4955-8922-ed49a41d5ef1")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Originators")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Originator;



		public static RequestInterface Instance {get; internal set;}

		internal RequestInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}