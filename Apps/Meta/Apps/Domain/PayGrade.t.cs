namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("028de4a4-12d4-422f-8d82-4f1edaa471ae")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]

	[Plural("PayGrades")]
	public partial class PayGradeClass : Class
	{
		#region Allors
		[Id("88ba9ad4-e7de-42d9-89d7-9292d34d308b")]
		[AssociationId("36e42e9c-a623-493f-a29b-a34cdf485612")]
		[RoleId("64944205-252c-49d7-8a59-771b4a8a4318")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("f7e52596-8814-48ff-a703-d80255110c5f")]
		[AssociationId("7ff0bc91-cc37-468d-b5ed-ae2de433acc8")]
		[RoleId("dc165e1f-88d2-4fb3-af0d-10d229f93528")]
		#endregion
		[Indexed]
		[Type(typeof(SalaryStepClass))]
		[Plural("SalarySteps")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType SalaryStep;



		public static PayGradeClass Instance {get; internal set;}

		internal PayGradeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}