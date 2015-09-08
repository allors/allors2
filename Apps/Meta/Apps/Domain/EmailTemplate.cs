namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c78a49b1-9918-4f15-95f3-c537c82f59fd")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("EmailTemplates")]
	public partial class EmailTemplateClass : Class
	{
		#region Allors
		[Id("21bbeaa8-f4cf-4b09-9fcd-af72a70e6f15")]
		[AssociationId("18d3ed19-fcac-4010-9bcb-2c0f6f41acc1")]
		[RoleId("27ade42e-f19f-444a-9134-db74add756b3")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("8bb431b6-a6ea-48d0-ad78-975ec26b470f")]
		[AssociationId("15e1b022-709b-4443-a85c-c1b2956c14e9")]
		[RoleId("8ce6a6a6-2387-4dd7-8bea-dec068aec152")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("BodyTemplates")]
		public RelationType BodyTemplate;

		#region Allors
		[Id("f05fc608-5dcd-4d7d-b472-5b84c2a195a4")]
		[AssociationId("c00233a0-c9a2-4c01-88fc-9ea5eb7fd564")]
		[RoleId("c39a94b3-455b-4602-8d55-abb2fca560ed")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("SubjectTemplates")]
		public RelationType SubjectTemplate;



		public static EmailTemplateClass Instance {get; internal set;}

		internal EmailTemplateClass() : base(MetaPopulation.Instance)
        {
        }
        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}