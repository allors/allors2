namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2be4075a-c7e3-4a38-a045-7910f85b3e46")]
	#endregion
	[Plural("ServiceEntryBillings")]
	public partial class ServiceEntryBillingClass : Class
	{
		#region Allors
		[Id("2fb9d650-0a28-4a39-8427-8c12bc4a20a1")]
		[AssociationId("c91e3796-6b23-4aa7-992b-ac15da334eae")]
		[RoleId("7ca6affa-7d4f-45bd-88e2-a0fa1cad4ad7")]
		#endregion
		[Indexed]
		[Type(typeof(ServiceEntryInterface))]
		[Plural("ServiceEntries")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ServiceEntry;

		#region Allors
		[Id("a8c707fb-98c1-43b1-99a3-9464cb25ea5f")]
		[AssociationId("284bf54c-8305-4892-ad00-f4975e155522")]
		[RoleId("570acec5-b62e-4abc-bb1d-000fb70bc2fe")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceItemInterface))]
		[Plural("InvoiceItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InvoiceItem;



		public static ServiceEntryBillingClass Instance {get; internal set;}

		internal ServiceEntryBillingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}