namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("959dd6ba-dfd5-4c7f-84f0-819fbef5c76a")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("CustomerReturnStatuses")]
	public partial class CustomerReturnStatusClass : Class
	{
		#region Allors
		[Id("7f02b626-26e6-43c7-af6f-44db32a9748a")]
		[AssociationId("88941e26-55e1-400a-925c-8b40977e8141")]
		[RoleId("6a362a6a-930f-4202-b9a7-07f6062c9dde")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("871dc477-7231-4180-b479-e66c0d2dbe58")]
		[AssociationId("b65cdd1a-423c-41b7-8978-a7cc4420166d")]
		[RoleId("3153feb0-9213-496e-b1ac-2ed5d6b431a8")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentReceiptClass))]
		[Plural("ShipmentReceipts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentReceipt;

		#region Allors
		[Id("eb96d9f9-cbbb-4c2e-84b6-6e4b17cc162f")]
		[AssociationId("2cf7c3d8-3915-4c41-b619-b317d3de7842")]
		[RoleId("f995313e-0317-4587-9f6e-456be2134f44")]
		#endregion
		[Indexed]
		[Type(typeof(CustomerReturnObjectStateClass))]
		[Plural("CustomerReturnObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CustomerReturnObjectState;



		public static CustomerReturnStatusClass Instance {get; internal set;}

		internal CustomerReturnStatusClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.CustomerReturnObjectState.RoleType.IsRequired = true;
        }
    }
}