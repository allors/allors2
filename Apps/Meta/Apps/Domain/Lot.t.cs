namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d900e278-7add-4e90-8bea-0a65d03f4fa7")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Lots")]
	public partial class LotClass : Class
	{
		#region Allors
		[Id("4888a06a-fcf5-42a7-a1c3-721d3abaa755")]
		[AssociationId("0f922c04-b617-4b72-8c22-02f43ac2afb9")]
		[RoleId("46b3ec4d-0463-48eb-8764-8dedf8c48b1a")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ExpirationDates")]
		public RelationType ExpirationDate;

		#region Allors
		[Id("8680f7e2-c5f1-43af-a127-68ac8404fbf4")]
		[AssociationId("e350d93d-c5ce-496b-a210-c01b4ff82c60")]
		[RoleId("92953ece-133e-4402-ad5c-5357c34bb99e")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Quantities")]
		public RelationType Quantity;

		#region Allors
		[Id("ca7a3e0f-e036-40ed-9346-0d1dae45c560")]
		[AssociationId("fdb9e9dc-1395-43ed-8234-187f35b8a7ef")]
		[RoleId("03e6a4fc-2336-4761-807f-20c1b5b80af0")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("LotNumbers")]
		public RelationType LotNumber;



		public static LotClass Instance {get; internal set;}

		internal LotClass() : base(MetaPopulation.Instance)
        {
        }
	}
}