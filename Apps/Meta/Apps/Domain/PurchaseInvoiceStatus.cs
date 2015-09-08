namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("622c8a98-ec26-4f05-9a09-a9032a41e586")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PurchaseInvoiceStatuses")]
	public partial class PurchaseInvoiceStatusClass : Class
	{
		#region Allors
		[Id("049dd047-7fa7-46e3-900b-84b87f960412")]
		[AssociationId("51ddc06d-aa92-49e1-b410-a7f69a474bdf")]
		[RoleId("bf01b58a-465e-4dec-a451-de4601d28850")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("1f5b8db1-58bd-4006-8878-f1609055149c")]
		[AssociationId("f63d8052-8f2f-47b5-a5f8-585b0b2587ae")]
		[RoleId("444ada74-4d76-4c78-a7c8-dc2c448dd7eb")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseInvoiceObjectStateClass))]
		[Plural("PurchaseInvoiceObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseInvoiceObjectState;



		public static PurchaseInvoiceStatusClass Instance {get; internal set;}

		internal PurchaseInvoiceStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}