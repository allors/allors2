namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("81ddff76-9b82-4309-9c9f-f7f9dbd2db21")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("WorkEffortGoodStandards")]
	public partial class WorkEffortGoodStandardClass : Class
	{
		#region Allors
		[Id("086907b1-97c2-47c1-ade4-f7749f615ae1")]
		[AssociationId("f3cf9c9b-2d69-4ef7-8240-44d1ca53bc6f")]
		[RoleId("cd4b1f0a-425f-43d3-bc00-d64a0c4e84df")]
		#endregion
		[Indexed]
		[Type(typeof(GoodClass))]
		[Plural("Goods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Good;

		#region Allors
		[Id("28b3b976-3354-4095-b928-7c1474e8c492")]
		[AssociationId("3b07f539-a06c-4cdc-8790-98c05e097aa6")]
		[RoleId("211ae475-665a-4677-a9eb-376ed9c4d886")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("EstimatedCosts")]
		public RelationType EstimatedCost;

		#region Allors
		[Id("c94d5e97-ec2b-4d32-ae8d-145595f0ad91")]
		[AssociationId("3ddc2478-34ba-45fa-aa21-a11c856fbfe0")]
		[RoleId("33be021f-3194-4e54-b69e-844814ca0bbd")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("EstimatedQuantities")]
		public RelationType EstimatedQuantity;



		public static WorkEffortGoodStandardClass Instance {get; internal set;}

		internal WorkEffortGoodStandardClass() : base(MetaPopulation.Instance)
        {
        }
	}
}