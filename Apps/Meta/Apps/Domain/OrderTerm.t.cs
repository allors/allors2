namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("86cf6a28-baeb-479d-ac9e-fabc7fe1994d")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("OrderTerms")]
	public partial class OrderTermClass : Class
	{
		#region Allors
		[Id("04cd1dd4-6f4f-4cd5-8ca0-5d3ccae06400")]
		[AssociationId("13b304b8-a945-4302-bd45-6a51f03aa8c9")]
		[RoleId("059b0064-a361-48d5-8340-f1ae43db454b")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("TermValues")]
		public RelationType TermValue;

		#region Allors
		[Id("0540ccac-4970-4026-9529-be62db0350a0")]
		[AssociationId("d5bc8696-24d9-408f-ba50-c20a2c43dec1")]
		[RoleId("76541960-6f11-4cd3-bc78-3018480cf742")]
		#endregion
		[Indexed]
		[Type(typeof(TermTypeClass))]
		[Plural("TermTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TermType;



		public static OrderTermClass Instance {get; internal set;}

		internal OrderTermClass() : base(MetaPopulation.Instance)
        {
        }
	}
}