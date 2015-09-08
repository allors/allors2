namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3aa7bf17-bd02-4587-9006-177845ae69df")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Responsibilities")]
	public partial class ResponsibilityClass : Class
	{
		#region Allors
		[Id("a570dd47-5bb6-4a37-b73e-3a9f7b3f37ee")]
		[AssociationId("0f98ce04-447c-497c-b63b-f943eb818c84")]
		[RoleId("9ccfe2ef-4980-43d8-9c5b-247c93c902b7")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static ResponsibilityClass Instance {get; internal set;}

		internal ResponsibilityClass() : base(MetaPopulation.Instance)
        {
        }
	}
}