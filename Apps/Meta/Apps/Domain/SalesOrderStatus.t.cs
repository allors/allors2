namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("347ee1c4-5275-4ea7-a349-6bab2de45aff")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("SalesOrderStatuses")]
	public partial class SalesOrderStatusClass : Class
	{
		#region Allors
		[Id("4c0986f4-c033-4646-b062-da9699bd8455")]
		[AssociationId("96c8b409-1ac9-4f31-a6a2-191bee4a1a82")]
		[RoleId("99a005bb-6961-4a19-bedc-5bdce1829cb9")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("e61dabc2-729c-41cc-8d89-aea6e6557914")]
		[AssociationId("f3b9f2c8-18b5-4334-99e5-7f4f4eee7571")]
		[RoleId("2e1c48fe-536b-4b2a-8e49-7320c961d42c")]
		#endregion
		[Indexed]
		[Type(typeof(SalesOrderObjectStateClass))]
		[Plural("SalesOrderObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesOrderObjectState;



		public static SalesOrderStatusClass Instance {get; internal set;}

		internal SalesOrderStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}