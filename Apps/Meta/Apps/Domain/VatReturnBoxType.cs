namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3b233161-d2a8-4d8f-a293-09d8a2bea3e2")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("VatReturnBoxTypes")]
	public partial class VatReturnBoxTypeClass : Class
	{
		#region Allors
		[Id("95935a8e-fac5-4798-ba2d-1408d231f97b")]
		[AssociationId("d40e1048-b97b-4bae-9319-f4c05ec40484")]
		[RoleId("44678a1f-9af2-404f-8eec-b50fb62737cb")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Types")]
		public RelationType Type;



		public static VatReturnBoxTypeClass Instance {get; internal set;}

		internal VatReturnBoxTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}