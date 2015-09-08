namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1f14fdb3-9e0f-4cea-b7c7-3ca2ab898f56")]
	#endregion
	[Plural("OrderItemsBilling")]
	public partial class OrderItemBillingClass : Class
	{
		#region Allors
		[Id("214988fc-b5a2-4944-9c83-93a645a96853")]
		[AssociationId("2007bddd-e78c-40a8-9015-5d3f027586c0")]
		[RoleId("624c3c0b-faac-4542-aeb2-466952cbf832")]
		#endregion
		[Indexed]
		[Type(typeof(OrderItemInterface))]
		[Plural("OrderItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType OrderItem;

		#region Allors
		[Id("23a0d52d-3ec7-4ddf-a300-c0ee46edf41a")]
		[AssociationId("03ac8386-6706-4e3f-9ad2-a64e67edf08f")]
		[RoleId("61e3ad81-395e-46e9-837f-e48257141164")]
		#endregion
		[Indexed]
		[Type(typeof(SalesInvoiceItemClass))]
		[Plural("SalesInvoiceItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesInvoiceItem;

		#region Allors
		[Id("2f75bdee-46f9-4dd0-b349-00a497462fdb")]
		[AssociationId("86dc1660-7719-4dae-93d8-ce4ca7a00f2a")]
		[RoleId("bc0b7bb6-c77b-451d-bf95-c32967766c49")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;

		#region Allors
		[Id("cfff23f0-1f3c-48a1-b4a7-85bc2254dbff")]
		[AssociationId("ed09cee4-3c01-4a2a-ab3d-6f9e8de16466")]
		[RoleId("6193e84f-882a-4b7a-b51c-8ec93f09aff2")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;



		public static OrderItemBillingClass Instance {get; internal set;}

		internal OrderItemBillingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}