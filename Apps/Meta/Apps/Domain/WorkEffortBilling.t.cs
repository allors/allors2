namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("15c8c72b-f551-41b0-86c8-80f02424ec4c")]
	#endregion
	[Plural("WorkEffortBillings")]
	public partial class WorkEffortBillingClass : Class
	{
		#region Allors
		[Id("3c83ca1d-b20e-4e8c-aa23-3bb03f421ba7")]
		[AssociationId("506b220c-7965-4d51-8413-feabfef71c07")]
		[RoleId("4d2f7ed8-881f-49e4-944a-ba291ec671d0")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortInterface))]
		[Plural("WorkEfforts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType WorkEffort;

		#region Allors
		[Id("91d38ce9-bf06-4272-bdd8-13401084223d")]
		[AssociationId("d0189269-2f90-46c5-a1ff-48bad8712b34")]
		[RoleId("e2a7d998-78bb-4d21-b4a8-d6fbddc4b089")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Percentages")]
		public RelationType Percentage;

		#region Allors
		[Id("c6ed6a42-6889-4ad9-b76a-22bd45e02e75")]
		[AssociationId("99eb5187-9c6b-48bf-a587-81a5d1603cb1")]
		[RoleId("977e55a2-1592-42ff-b7a2-9f1630b36714")]
		#endregion
		[Indexed]
		[Type(typeof(InvoiceItemInterface))]
		[Plural("InvoiceItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InvoiceItem;



		public static WorkEffortBillingClass Instance {get; internal set;}

		internal WorkEffortBillingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}