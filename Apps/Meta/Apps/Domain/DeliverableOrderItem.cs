namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("66bd584c-37c4-4969-874b-7a459195fd25")]
	#endregion
	[Inherit(typeof(EngagementItemInterface))]

	[Plural("DeliverableOrderItems")]
	public partial class DeliverableOrderItemClass : Class
	{
		#region Allors
		[Id("f9e13dab-0081-4d25-8021-f5ed5bef5f0e")]
		[AssociationId("86376834-b792-425e-a21d-30065dca6dd4")]
		[RoleId("fb6ba6e4-2f9f-4230-b536-df8e305797f9")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AgreedUponPrices")]
		public RelationType AgreedUponPrice;



		public static DeliverableOrderItemClass Instance {get; internal set;}

		internal DeliverableOrderItemClass() : base(MetaPopulation.Instance)
        {
        }
	}
}