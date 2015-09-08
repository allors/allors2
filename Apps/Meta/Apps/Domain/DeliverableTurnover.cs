namespace Allors.Meta
{
	#region Allors
	[Id("48733d8e-506a-4add-a230-907221ca7a9a")]
	#endregion
	[Inherit(typeof(ServiceEntryInterface))]
	public partial class DeliverableTurnoverClass : Class
	{
		#region Allors
		[Id("5c9b7809-0cb0-4282-ae2b-20407126384d")]
		[AssociationId("8e050223-57c1-47b2-b5b4-bdb93840f527")]
		[RoleId("8d3abfcb-f4de-4d6b-9427-b7906430a178")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;

		public static DeliverableTurnoverClass Instance {get; internal set;}

		internal DeliverableTurnoverClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Amount.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ServiceEntryInterface.Instance.Description.RoleType].IsRequiredOverride = true;
        }
    }
}