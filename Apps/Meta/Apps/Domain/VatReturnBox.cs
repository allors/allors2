namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8dc67774-c15a-47dd-9b8a-ce4e7139e8a3")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("VatReturnBoxes")]
	public partial class VatReturnBoxClass : Class
	{
		#region Allors
		[Id("3bcc4fc9-5646-4ceb-b48b-bb1d7fbcba64")]
		[AssociationId("69f949c3-f5c1-4cb4-a907-ce3673496628")]
		[RoleId("ec126f8a-4d48-4c1e-bdb6-ad66ab529580")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("HeaderNumbers")]
		public RelationType HeaderNumber;

		#region Allors
		[Id("78e114b4-ec1d-49ce-ab32-40a3184dea31")]
		[AssociationId("98920876-b4f8-4d41-90f1-115164441836")]
		[RoleId("9a8717dd-0713-458b-8d84-9758f4ddfb03")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static VatReturnBoxClass Instance {get; internal set;}

		internal VatReturnBoxClass() : base(MetaPopulation.Instance)
        {
        }
	}
}