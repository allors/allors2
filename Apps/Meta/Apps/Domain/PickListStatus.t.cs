namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("563c9706-0b34-4bf0-a09f-72881f10fe6c")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PickListStatuses")]
	public partial class PickListStatusClass : Class
	{
		#region Allors
		[Id("e1187cc2-9518-4387-986a-e989b303035f")]
		[AssociationId("b47b4537-e686-4f86-b45f-5366f05de7d3")]
		[RoleId("67ffe9b3-3916-48e3-9c64-d1427f350737")]
		#endregion
		[Indexed]
		[Type(typeof(PickListObjectStateClass))]
		[Plural("PickListObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PickListObjectState;

		#region Allors
		[Id("f87a3dcf-742c-4a3c-afbb-af1969164db9")]
		[AssociationId("153a2b44-da58-4db4-9b57-bfa9992c0353")]
		[RoleId("52862edb-477c-4522-85c8-bcedb6affcdd")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static PickListStatusClass Instance {get; internal set;}

		internal PickListStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}