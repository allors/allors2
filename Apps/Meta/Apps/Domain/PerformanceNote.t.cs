namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4629c7ed-e9a4-4f31-bb46-e3f2920bd768")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]

	[Plural("PerformanceNotes")]
	public partial class PerformanceNoteClass : Class
	{
		#region Allors
		[Id("1b8f0ada-bb5c-4226-8e35-5f1c40b06fc8")]
		[AssociationId("e4ae1691-22f8-4304-8e04-73ae41420b43")]
		[RoleId("1d396f6f-279d-4b83-9d95-6ece6089f6a0")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("2f6ed687-4200-4a27-bfb2-922d9ce2e38f")]
		[AssociationId("5f2b047e-2cb0-4d2a-9cce-77846ad35f45")]
		[RoleId("f21bbf2d-0780-4bbf-92e6-2c6676b4893d")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("CommunicationDates")]
		public RelationType CommunicationDate;

		#region Allors
		[Id("5bf234d2-8486-47b2-a770-eca36b44bb67")]
		[AssociationId("cc9f9a6f-54fc-4786-9d83-2769d8d921ce")]
		[RoleId("0467f9fa-17e9-4fdc-b74a-39d074e55b16")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("GivenByManagers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GivenByManager;

		#region Allors
		[Id("a8cd7bf6-6bea-44ad-9e89-1bd63ffca459")]
		[AssociationId("c4a4e475-613b-4e38-bb79-b5bd12f73332")]
		[RoleId("06b721ea-20ec-4b18-bd5c-d6d3e86610bd")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Employees")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Employee;



		public static PerformanceNoteClass Instance {get; internal set;}

		internal PerformanceNoteClass() : base(MetaPopulation.Instance)
        {
        }
	}
}