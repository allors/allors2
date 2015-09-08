namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("15f511a9-7a08-42e4-a690-e0d2f01c9686")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PurchaseReturnStatuses")]
	public partial class PurchaseReturnStatusClass : Class
	{
		#region Allors
		[Id("1485de84-2719-4543-9250-807f1a9e60bf")]
		[AssociationId("9694eb68-d7c4-40db-b15d-deacb698e976")]
		[RoleId("48bd4dce-ecae-4acf-b820-70352958c04b")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseReturnObjectStateClass))]
		[Plural("PurchaseReturnObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseReturnObjectState;

		#region Allors
		[Id("46004f49-9d2e-49a6-870e-c81f44458c59")]
		[AssociationId("3b2a6ce5-a5c8-45c3-ba3b-3fa5326e4293")]
		[RoleId("bd72c728-57ef-4e68-9bd1-2517bfe7e972")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static PurchaseReturnStatusClass Instance {get; internal set;}

		internal PurchaseReturnStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}