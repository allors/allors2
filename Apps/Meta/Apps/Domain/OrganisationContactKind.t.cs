namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9570d60a-8baa-439c-99f4-472d10952165")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

	[Plural("OrganisationContactKinds")]
	public partial class OrganisationContactKindClass : Class
	{
		#region Allors
		[Id("5d3446a3-ab54-4c49-89bb-928b082bb4b7")]
		[AssociationId("a1b7eec7-d13f-47da-b028-4db580da07a4")]
		[RoleId("291d3e15-301a-4865-9097-5407dadd65ff")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static OrganisationContactKindClass Instance {get; internal set;}

		internal OrganisationContactKindClass() : base(MetaPopulation.Instance)
        {
        }
	}
}