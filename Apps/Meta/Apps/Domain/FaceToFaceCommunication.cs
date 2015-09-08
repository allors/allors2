namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d0f9fc0d-a3c5-46cc-ab00-4c724995fc14")]
	#endregion
	[Inherit(typeof(CommunicationEventInterface))]

	[Plural("FaceToFaceCommunications")]
	public partial class FaceToFaceCommunicationClass : Class
	{
		#region Allors
		[Id("52b8614b-799e-4aea-a012-ea8dbc23f8dd")]
		[AssociationId("ac424847-d426-4614-99a2-37c70841c454")]
		[RoleId("bcf4a8df-8b57-4b3c-a6e5-f9b56c71a13b")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Participants")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Participant;

		#region Allors
		[Id("95ae979f-d549-4ea1-87f0-46aa55e4b14a")]
		[AssociationId("d34e4203-0bd2-4fe4-a2ef-9f9f52b49cf9")]
		[RoleId("9f67b296-953d-4e04-b94d-6ffece87ceef")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Locations")]
		public RelationType Location;



		public static FaceToFaceCommunicationClass Instance {get; internal set;}

		internal FaceToFaceCommunicationClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Participant.RoleType.IsRequired = true;
        }
    }
}