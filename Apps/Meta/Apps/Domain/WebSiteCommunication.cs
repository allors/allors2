namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ecf2996a-7f8b-45d5-afac-56c88c62136a")]
	#endregion
	[Inherit(typeof(CommunicationEventInterface))]

	[Plural("WebSiteCommunications")]
	public partial class WebSiteCommunicationClass : Class
	{
		#region Allors
		[Id("18faf993-316a-4990-8ffd-8bda40f61164")]
		[AssociationId("b6c8df26-71f6-49a8-86d0-f38b9717fdc4")]
		[RoleId("96f92902-be8e-41f8-893a-afe4e93ef6d5")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Originators")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Originator;

		#region Allors
		[Id("39077571-13b2-4cc4-be85-517dbc11703e")]
		[AssociationId("be25f23d-6c17-4940-abe6-b6936244bcea")]
		[RoleId("f956749a-b0b3-45a4-a4b8-b0bf913d24c2")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Receivers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Receiver;



		public static WebSiteCommunicationClass Instance {get; internal set;}

		internal WebSiteCommunicationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}