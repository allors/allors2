namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("528cf616-6c67-42e1-af69-b5e6cb1192ea")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("LegalForms")]
	public partial class LegalFormClass : Class
	{
		#region Allors
		[Id("2867d3b0-5def-4fc6-880a-be4bfe1d2597")]
		[AssociationId("ee4e44e3-2f9b-45fc-8b79-f2ac8e2da434")]
		[RoleId("7aa44ba6-a0b4-403b-aabb-7622ddd2db30")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static LegalFormClass Instance {get; internal set;}

		internal LegalFormClass() : base(MetaPopulation.Instance)
        {
        }
	}
}