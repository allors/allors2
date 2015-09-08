namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("37de59b2-ca6c-4fa9-86a2-299fd6f14812")]
	#endregion
	[Inherit(typeof(WorkEffortInterface))]

	[Plural("ProductionRuns")]
	public partial class ProductionRunClass : Class
	{
		#region Allors
		[Id("108bb811-ece8-42b4-89e2-7a394f848f4d")]
		[AssociationId("8eeef339-d38c-45c4-8300-61bbb33cb205")]
		[RoleId("abbbac9a-74f3-4e7d-a56e-f5ba0c967530")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("QuantitiesProduced")]
		public RelationType QuantityProduced;

		#region Allors
		[Id("407b8671-79ea-4998-b5ed-188dd4a9f43c")]
		[AssociationId("7358c0de-4918-4998-afb8-ecd122e04e3a")]
		[RoleId("56da6402-ddd9-4bbb-83be-3c368de22d09")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("QuantitiesRejected")]
		public RelationType QuantityRejected;

		#region Allors
		[Id("558dfd44-26a5-4d64-9317-a121fabaecf1")]
		[AssociationId("69036ddb-1a7b-4bce-8ee2-2610715e47c0")]
		[RoleId("a708d61a-08d1-45db-877b-3eb4514a9069")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("QuantitiesToProduce")]
		public RelationType QuantityToProduce;



		public static ProductionRunClass Instance {get; internal set;}

		internal ProductionRunClass() : base(MetaPopulation.Instance)
        {
        }
	}
}