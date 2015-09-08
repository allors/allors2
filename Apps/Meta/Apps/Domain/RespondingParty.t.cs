namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4b1e9776-8851-4a2a-a402-1b40211d1f3b")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("RespondingParties")]
	public partial class RespondingPartyClass : Class
	{
		#region Allors
		[Id("13f84c6c-d44a-4cc2-8898-bc2cbaed04f4")]
		[AssociationId("88a8016f-ecd7-4085-82d0-a9698d078184")]
		[RoleId("72177a66-0459-4b6e-a8ea-2b5786e09f31")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("SendingDates")]
		public RelationType SendingDate;

		#region Allors
		[Id("1d220b47-44de-4ab9-9219-b3acf78bdaf2")]
		[AssociationId("5d99c05d-6fea-456e-a6a5-9ba6b6a7ab7f")]
		[RoleId("90f6944e-1b82-4fdf-8594-02149a063d7e")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("ContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ContactMechanism;

		#region Allors
		[Id("8e4080f7-40b7-437c-aff2-0fb6b809797a")]
		[AssociationId("8f61bcf0-a51c-4c02-95a8-99376824f5ab")]
		[RoleId("79384094-3720-418c-8b87-66084af7fa11")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Parties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Party;



		public static RespondingPartyClass Instance {get; internal set;}

		internal RespondingPartyClass() : base(MetaPopulation.Instance)
        {
        }
	}
}