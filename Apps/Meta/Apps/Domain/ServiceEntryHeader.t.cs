namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("22e85314-cfdf-4ead-a816-18588294fa79")]
	#endregion
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("ServiceEntryHeaders")]
	public partial class ServiceEntryHeaderClass : Class
	{
		#region Allors
		[Id("6b29a626-04f6-423f-8ae5-cb49e8f9211d")]
		[AssociationId("9f14e67f-328b-44e6-8c80-707441848265")]
		[RoleId("21500c76-8a3e-4737-aa69-e348e06440e2")]
		#endregion
		[Indexed]
		[Type(typeof(ServiceEntryInterface))]
		[Plural("ServiceEntries")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ServiceEntry;

		#region Allors
		[Id("7e160fbc-1339-433c-9dcb-9b3ad58ad400")]
		[AssociationId("a9d0cbd8-bb20-45e1-b109-6620b23fa1b7")]
		[RoleId("ef8b435e-e354-45e2-89bc-3d452cc84f5a")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("SubmittedDates")]
		public RelationType SubmittedDate;

		#region Allors
		[Id("902235fe-a6c5-47bb-936b-8b6ce54b3d15")]
		[AssociationId("1f93dde3-a9bd-4e10-8ec6-38edaec6ffb5")]
		[RoleId("3b27dd30-5452-480f-ae19-6937c422b541")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SubmittedBys")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SubmittedBy;



		public static ServiceEntryHeaderClass Instance {get; internal set;}

		internal ServiceEntryHeaderClass() : base(MetaPopulation.Instance)
        {
        }
	}
}