namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("92b62390-9bf9-432b-b81e-242a5467e10e")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PurchaseOrderStatuses")]
	public partial class PurchaseOrderStatusClass : Class
	{
		#region Allors
		[Id("1f6f2902-0b17-4537-970d-a72454b91410")]
		[AssociationId("b3e0bc80-0b37-4946-a6c1-e84cb522e949")]
		[RoleId("95d7759c-aa52-4b38-8337-0c59287441aa")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseOrderObjectStateClass))]
		[Plural("PurchaseOrderObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseOrderObjectState;

		#region Allors
		[Id("fe949e5f-b717-4cda-8f40-5b0db57d43dd")]
		[AssociationId("890b82dc-5a8b-463b-bcc4-cccee90b8dfb")]
		[RoleId("e505854a-67de-4cdf-8404-a0495178ed74")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static PurchaseOrderStatusClass Instance {get; internal set;}

		internal PurchaseOrderStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}