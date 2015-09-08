namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9e679821-8eeb-4dce-b090-d8ade95cb47f")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]

	[Plural("ProfessionalAssignments")]
	public partial class ProfessionalAssignmentClass : Class
	{
		#region Allors
		[Id("18af73aa-336f-4120-8508-a59a9acf17bc")]
		[AssociationId("31da78aa-5e06-48f8-90e4-018ef021a280")]
		[RoleId("cf515b68-b198-4348-881c-fd9a0bcf22bf")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Professionals")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Professional;

		#region Allors
		[Id("a75d3ec2-c4f8-4de6-a10c-fe5e3897e663")]
		[AssociationId("70e8f936-27c8-42cb-9459-9a823aaa6318")]
		[RoleId("bb592768-a6f0-47fb-bc74-a15fc5867b34")]
		#endregion
		[Indexed]
		[Type(typeof(EngagementItemInterface))]
		[Plural("EngagementItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EngagementItem;



		public static ProfessionalAssignmentClass Instance {get; internal set;}

		internal ProfessionalAssignmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}