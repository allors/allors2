namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2b5efcb9-54ba-4d59-833b-716d321cc7cb")]
	#endregion
	[Plural("EventRegistrations")]
	public partial class EventRegistrationClass : Class
	{
		#region Allors
		[Id("af4b8828-bea1-43e5-b109-9934311cc2df")]
		[AssociationId("bd962b63-dff5-4b09-bd30-ef11755a381e")]
		[RoleId("dcfcbf03-1cac-4533-a3b7-a235f167645c")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Persons")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Person;

		#region Allors
		[Id("ed542026-7020-43e3-ab72-c3f4dd991a4b")]
		[AssociationId("cba93e20-e78d-4c86-8b5b-2daa930fde35")]
		[RoleId("b2c0c98f-5ed7-4329-bfcb-ab0f3e6e169c")]
		#endregion
		[Indexed]
		[Type(typeof(EventClass))]
		[Plural("Events")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Event;

		#region Allors
		[Id("f9805362-2bd2-46d4-b9b2-d38cd0a76f78")]
		[AssociationId("e2dcb678-d8ba-45ed-9f25-5a87aae2d18f")]
		[RoleId("19f9c2a6-5e2c-4bfb-bc63-b13d998e92e3")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("AllorsDateTimes")]
		public RelationType AllorsDateTime;



		public static EventRegistrationClass Instance {get; internal set;}

		internal EventRegistrationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}