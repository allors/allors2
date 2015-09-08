namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("013d3508-a663-4af5-ba01-24b7b907f581")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("SalesInvoiceItemStatuses")]
	public partial class SalesInvoiceItemStatusClass : Class
	{
		#region Allors
		[Id("2d0de395-ccfe-46b4-9391-020c276ddafd")]
		[AssociationId("a4d34c93-13ee-449f-b016-47707c7ae72d")]
		[RoleId("054b768e-78ae-44e9-939f-765fe5c4ccf4")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("ea2f0286-38d6-42ad-825a-a692e51cd209")]
		[AssociationId("0b1f32c0-e8de-4d3e-9de8-e172c1477ada")]
		[RoleId("54b5248a-d692-4c78-9cd9-b6a9e7bc3e34")]
		#endregion
		[Indexed]
		[Type(typeof(SalesInvoiceItemObjectStateClass))]
		[Plural("SalesInvoiceItemObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesInvoiceItemObjectState;



		public static SalesInvoiceItemStatusClass Instance {get; internal set;}

		internal SalesInvoiceItemStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}