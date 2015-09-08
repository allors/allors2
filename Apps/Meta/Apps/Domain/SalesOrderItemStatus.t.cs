namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("de70746f-2c82-4f01-8de9-b4f78105426a")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("SalesOrderItemStatuses")]
	public partial class SalesOrderItemStatusClass : Class
	{
		#region Allors
		[Id("90dd5f56-af80-4f78-a0b6-c13f34c87193")]
		[AssociationId("05167075-7e33-42ba-a40a-ef2233af019a")]
		[RoleId("3b0a60b9-b8f3-41f0-af4f-75598240bde1")]
		#endregion
		[Indexed]
		[Type(typeof(SalesOrderItemObjectStateClass))]
		[Plural("SalesOrderItemObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesOrderItemObjectState;

		#region Allors
		[Id("f31a9949-c0b3-45c5-854f-29884ce45c9b")]
		[AssociationId("73152227-ec98-4b5a-9fad-0e2d38cd7c61")]
		[RoleId("47852a3a-f906-4757-87b9-29c6e8d560f9")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static SalesOrderItemStatusClass Instance {get; internal set;}

		internal SalesOrderItemStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}