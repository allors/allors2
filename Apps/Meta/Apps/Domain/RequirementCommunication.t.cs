namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("49cdc4a2-f7af-43c9-b160-4c7da9a0ca42")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("RequirementCommunications")]
	public partial class RequirementCommunicationClass : Class
	{
		#region Allors
		[Id("5a4d9541-4a8a-4661-bec3-e65db5298857")]
		[AssociationId("d7103ab4-c796-4efd-83bd-256e90c40a14")]
		[RoleId("8edb2d05-b8aa-4d09-90ef-79ce9051df66")]
		#endregion
		[Indexed]
		[Type(typeof(CommunicationEventInterface))]
		[Plural("CommunicationEvents")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CommunicationEvent;

		#region Allors
		[Id("b65140b1-8dc4-4836-9ad8-fe01f43dad7a")]
		[AssociationId("b2ddd7e5-fa91-4257-9400-f776787fffb7")]
		[RoleId("09fb424a-eece-4617-bc65-9fb6861eeb3b")]
		#endregion
		[Indexed]
		[Type(typeof(RequirementInterface))]
		[Plural("Requirements")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Requirement;

		#region Allors
		[Id("cdb72b3f-9920-4082-83a7-a0211a29cf77")]
		[AssociationId("f0743736-d40a-4831-a075-8bdd33cb68f6")]
		[RoleId("208ee5d1-7f60-4c12-888f-04f25c38bc46")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("AssociatedProfessionals")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType AssociatedProfessional;



		public static RequirementCommunicationClass Instance {get; internal set;}

		internal RequirementCommunicationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}