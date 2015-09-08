namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("eba70b57-05e3-487f-8cf1-45f14fcdc3b9")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("VatForms")]
	public partial class VatFormClass : Class
	{
		#region Allors
		[Id("180f9887-5973-4c4a-9277-a383e4f66bc6")]
		[AssociationId("db1bf9e9-dc26-40e1-aa5d-c863955e2947")]
		[RoleId("5a3a106c-8a5e-4a4b-b86e-311853aa4353")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("f3683ece-e247-490f-be4f-4fe12e5cfd06")]
		[AssociationId("e8a9518b-d33b-4db5-ac01-6283028a7e1f")]
		[RoleId("657b667e-cd15-4671-bc18-9f49c8aa04e6")]
		#endregion
		[Indexed]
		[Type(typeof(VatReturnBoxClass))]
		[Plural("VatReturnBoxes")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType VatReturnBox;



		public static VatFormClass Instance {get; internal set;}

		internal VatFormClass() : base(MetaPopulation.Instance)
        {
        }
	}
}