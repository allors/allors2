namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1d157965-59b5-4ead-b4e4-c722495d7658")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PartySkills")]
	public partial class PartySkillClass : Class
	{
		#region Allors
		[Id("3254f43d-7b3a-49c8-8c1b-19fa0e4f6901")]
		[AssociationId("a25f511d-a4f9-4360-9150-304ed62d411f")]
		[RoleId("a6c023ba-549c-4895-bd32-ed70f05ef121")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("YearsExperiences")]
		public RelationType YearsExperience;

		#region Allors
		[Id("7ed819c8-78ef-4fe3-b499-b381c246711f")]
		[AssociationId("4a88ee23-2c4a-41d2-9891-77e5086db97d")]
		[RoleId("ecb7eb99-dc8f-4ca0-9744-fb87a708430a")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartedUsingDates")]
		public RelationType StartedUsingDate;

		#region Allors
		[Id("a341478c-503c-49ee-8c9a-e85b777e9ff4")]
		[AssociationId("0a9d48d6-e307-461d-b30f-14deae3d5bd8")]
		[RoleId("cd88bbe3-aa6a-4051-a800-57e685d85587")]
		#endregion
		[Type(typeof(SkillRatingClass))]
		[Plural("SkillRatings")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType SkillRating;

		#region Allors
		[Id("eb3e02dc-6ee5-4aca-9f35-68edafed6dd2")]
		[AssociationId("9223e489-7115-4765-88fd-b18f0d7e8c28")]
		[RoleId("5d9c639a-7c94-4771-ad37-be6e4b68dd06")]
		#endregion
		[Indexed]
		[Type(typeof(SkillLevelClass))]
		[Plural("SkillLevels")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SkillLevel;

		#region Allors
		[Id("fec11de5-a33c-4dd7-9af9-b32c3889c8a3")]
		[AssociationId("9c16c4b8-b80f-478f-96b0-a534f9de5663")]
		[RoleId("9728a273-f8d7-4edd-94ff-7a91d178fe82")]
		#endregion
		[Indexed]
		[Type(typeof(SkillClass))]
		[Plural("Skills")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Skill;



		public static PartySkillClass Instance {get; internal set;}

		internal PartySkillClass() : base(MetaPopulation.Instance)
        {
        }
	}
}