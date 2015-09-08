namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4a3efb9c-1556-4e57-bb59-f09d297e607e")]
	#endregion
	[Plural("FixedAssets")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class FixedAssetInterface: Interface
	{
		#region Allors
		[Id("354107ce-4eb6-4b9a-83c8-5cfe5e3adb22")]
		[AssociationId("e0f80027-f068-4ff8-a351-b3199f92735f")]
		[RoleId("6806756e-a152-42a9-b32b-b14269e712e2")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("51133e4d-5135-4991-9f2f-8df9762fac78")]
		[AssociationId("fc2144b7-4a88-412d-9792-ba6f6c93c637")]
		[RoleId("1cc0737e-a810-48d3-b048-7e3077d3db5c")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("LastServiceDates")]
		public RelationType LastServiceDate;

		#region Allors
		[Id("54cf9225-9204-43ee-9984-7fd8b2cbf8bc")]
		[AssociationId("efb718b5-7d70-4696-81c8-961582ed01f2")]
		[RoleId("99c0a722-af34-4008-b7f5-dc4315c7fa1a")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("AcquiredDates")]
		public RelationType AcquiredDate;

		#region Allors
		[Id("725c6b7d-68ed-4576-8b17-eac4e9f4db83")]
		[AssociationId("ce93a11b-7c87-4d9c-9d79-a9703a9fd86d")]
		[RoleId("96524022-ff94-482a-a17c-6c3c96f79127")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("913cc338-f844-49ae-886a-2e32db190b78")]
		[AssociationId("276b6fca-d2bb-4e43-af51-378c599c80f6")]
		[RoleId("f409664f-5c7e-4f3b-809c-acd43c36b3bc")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ProductionCapacities")]
		public RelationType ProductionCapacity;

		#region Allors
		[Id("ead0e86a-dfc7-45b0-9865-b973175c4567")]
		[AssociationId("6be614a2-0511-4ca0-9b1c-c8a3d0b0a998")]
		[RoleId("47d9d93c-8ba3-4f28-a8a5-6a4cb02853e6")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("NextServiceDates")]
		public RelationType NextServiceDate;



		public static FixedAssetInterface Instance {get; internal set;}

		internal FixedAssetInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}