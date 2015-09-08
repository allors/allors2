namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1e67320b-9680-4477-bf1b-70ccd24ab758")]
	#endregion
	[Inherit(typeof(CommunicationEventInterface))]

	[Plural("FaxCommunications")]
	public partial class FaxCommunicationClass : Class
	{
		#region Allors
		[Id("3c4bea84-e00e-4ab3-8d40-5de7f394e835")]
		[AssociationId("30a33d23-6c06-45cc-8cef-25a2d02cfc5f")]
		[RoleId("c3ad4d30-c9ef-41da-b7de-f71c625b8549")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Originators")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Originator;

		#region Allors
		[Id("79ec572e-b4a2-4a33-90c3-65c9f9e4012c")]
		[AssociationId("2a477a7f-bc36-437c-97df-dfca39236eb5")]
		[RoleId("2e213178-fe72-4258-a8f5-ff926f8e5591")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Receivers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Receiver;

		#region Allors
		[Id("8797fd5b-0d89-420f-b656-aff35b50e75c")]
		[AssociationId("42e2cb18-3596-443c-876c-3e557189ef2a")]
		[RoleId("7c820d65-87d3-4be3-be2e-8fa6a8b13a97")]
		#endregion
		[Indexed]
		[Type(typeof(TelecommunicationsNumberClass))]
		[Plural("OutgoingFaxNumbers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType OutgoingFaxNumber;



		public static FaxCommunicationClass Instance {get; internal set;}

		internal FaxCommunicationClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Originator.RoleType.IsRequired = true;
            this.Receiver.RoleType.IsRequired = true;
        }
    }
}