namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0eaa8719-bbf4-408a-8226-851580556024")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Trainings")]
	public partial class TrainingClass : Class
	{
		#region Allors
		[Id("7d2e7956-fb60-4a1b-8e5f-ee88b1b8e3b7")]
		[AssociationId("ff4c2753-ce42-4aa8-b1b1-3486e6aa11d9")]
		[RoleId("ee47ec51-a1d0-4d12-97cc-5a089869caa6")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static TrainingClass Instance {get; internal set;}

		internal TrainingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}