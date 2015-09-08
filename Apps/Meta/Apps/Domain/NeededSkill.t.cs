namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5e31a968-5f7d-4109-9821-b94459f13382")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("NeededSkills")]
	public partial class NeededSkillClass : Class
	{
		#region Allors
		[Id("079ef934-26e1-4dba-a69a-73fcc22d380e")]
		[AssociationId("f2afa9e5-239d-46c8-94c7-57dd23cb645a")]
		[RoleId("90f27ec7-03b8-491b-862c-3c18a37d4dbc")]
		#endregion
		[Indexed]
		[Type(typeof(SkillLevelClass))]
		[Plural("SkillLevels")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SkillLevel;

		#region Allors
		[Id("21207c09-22b0-469a-84a7-6edd300c73f7")]
		[AssociationId("a2c931e4-8200-4cdd-9d26-bedbaf529c29")]
		[RoleId("1984780a-81fa-4391-af4e-20f707550a3d")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("YearsExperiences")]
		public RelationType YearsExperience;

		#region Allors
		[Id("590d749a-52d4-448a-8f95-8412c0115825")]
		[AssociationId("3e6cc798-dae0-4381-abfd-bcba0b449d03")]
		[RoleId("09e6d6b8-8a89-46af-99fa-f332fea7ab6c")]
		#endregion
		[Indexed]
		[Type(typeof(SkillClass))]
		[Plural("Skills")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Skill;



		public static NeededSkillClass Instance {get; internal set;}

		internal NeededSkillClass() : base(MetaPopulation.Instance)
        {
        }
	}
}