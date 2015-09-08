namespace Allors.Meta
{
	#region Allors
	[Id("96a64894-e444-4df4-9289-1b121842ac73")]
	#endregion
	[Inherit(typeof(PriceComponentInterface))]

	[Plural("UtilizationCharges")]
	public partial class UtilizationChargeClass : Class
	{
		#region Allors
		[Id("3a371680-fc37-44dc-b3be-cdd76b6dd1e4")]
		[AssociationId("15d9f938-5a5c-472c-92a6-6769a37f652c")]
		[RoleId("a1e57ec7-561d-4c8e-8652-aea06598fb1b")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;

		#region Allors
		[Id("4f933f12-1337-453c-9cfd-6babaf9189d5")]
		[AssociationId("b49286b4-db2a-4025-8fb2-9390514b69dc")]
		[RoleId("037bba17-d291-40ea-920b-f09995ef04fb")]
		#endregion
		[Indexed]
		[Type(typeof(UnitOfMeasureClass))]
		[Plural("UnitsOfMeasure")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType UnitOfMeasure;

		public static UtilizationChargeClass Instance {get; internal set;}

		internal UtilizationChargeClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[PriceComponentInterface.Instance.Price.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[PriceComponentInterface.Instance.Description.RoleType].IsRequiredOverride = true;
        }
    }
}