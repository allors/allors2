namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7d3a207b-dbdd-48c4-9a92-8b12e4e77874")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("SalesInvoiceStatuses")]
	public partial class SalesInvoiceStatusClass : Class
	{
		#region Allors
		[Id("22daba0b-1f86-4a00-ba83-c541e65822c6")]
		[AssociationId("d28b067f-bd90-45c5-9213-b231ff3bb03f")]
		[RoleId("eb1505fb-6caa-40a3-a09c-1b18fe7dc3ee")]
		#endregion
		[Indexed]
		[Type(typeof(SalesInvoiceObjectStateClass))]
		[Plural("SalesInvoiceObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesInvoiceObjectState;

		#region Allors
		[Id("74c60d54-b75f-4baa-b1d6-5a33e8ab3944")]
		[AssociationId("ea6bf951-414e-48e6-a579-a2ce8627f635")]
		[RoleId("22405cc3-c402-4236-9517-bdb381d3285f")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static SalesInvoiceStatusClass Instance {get; internal set;}

		internal SalesInvoiceStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}