namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("05964e28-2c1d-4837-a887-2255f157e889")]
	#endregion
	[Inherit(typeof(CommunicationEventInterface))]

	[Plural("LetterCorrespondences")]
	public partial class LetterCorrespondenceClass : Class
	{
		#region Allors
		[Id("3e0f1be5-0685-48d6-922f-6e971110b414")]
		[AssociationId("d063c86e-bbee-41b9-9823-10e96c69c5a0")]
		[RoleId("14ca37a9-7ce4-4d2a-b7ba-1a43bccc1664")]
		#endregion
		[Indexed]
		[Type(typeof(PostalAddressClass))]
		[Plural("PostalAddresses")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType PostalAddress;

		#region Allors
		[Id("e8fd2c39-bcb7-4914-8cd3-6dcc6a7a9997")]
		[AssociationId("d5ed6948-f657-4d47-89c8-d860e2971138")]
		[RoleId("b65552b5-99c7-4b91-b9b6-a70ec35c3ae2")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Originators")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Originator;

		#region Allors
		[Id("ece02647-000a-4373-8f01-f4b7d1c75dd5")]
		[AssociationId("e580ed8f-a7a4-40c3-9c0a-4cdbe95354a6")]
		[RoleId("dde368dc-c198-4744-b3b2-1a2e0d2976e4")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Receivers")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Receiver;



		public static LetterCorrespondenceClass Instance {get; internal set;}

		internal LetterCorrespondenceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}